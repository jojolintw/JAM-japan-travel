using JP_FrontWebAPI.DTOs.Member;
using JP_FrontWebAPI.DTOs.Shared;
using JP_FrontWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using prjJapanTravel_BackendMVC.ViewModels.Ports;
using prjJapanTravel_BackendMVC.ViewModels.ShipmentViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace JP_FrontWebAPI.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : Controller
    {
        private JapanTravelContext _context;
        public ShipmentController(JapanTravelContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShipmentListViewModel>>> GetShipments(
    string? originPort = null,
    string? destinationPort = null,
    string? sortBy = null,
    bool isAscending = true,
    int pageNumber = 1,
    int pageSize = 12)
        {
            var query = _context.Routes
                .Include(r => r.OriginPort)
                .Include(r => r.DestinationPort)
                .Include(r => r.Schedules) // Include schedules for next departure calculation
                .AsQueryable();

            // 排序設定
            query = sortBy switch
            {
                "priceAsc" => query.OrderBy(r => r.Price),
                "priceDesc" => query.OrderByDescending(r => r.Price),
                "latest" => query.OrderByDescending(r => r.RouteId),
                _ => isAscending ? query.OrderBy(r => r.RouteId) : query.OrderByDescending(r => r.RouteId)
            };

            // 過濾出發港和目的港
            if (!string.IsNullOrEmpty(originPort))
            {
                query = query.Where(r => r.OriginPort.PortName.Contains(originPort));
            }
            if (!string.IsNullOrEmpty(destinationPort))
            {
                query = query.Where(r => r.DestinationPort.PortName.Contains(destinationPort));
            }

            var totalRecords = await query.CountAsync();
            var shipments = await query
                     .Skip((pageNumber - 1) * pageSize)
                     .Take(pageSize)
                     .Select(r => new ShipmentListViewModel
                     {
                         RouteId = r.RouteId,
                         OriginPortName = r.OriginPort.PortName,
                         DestinationPortName = r.DestinationPort.PortName,
                         Price = r.Price,
                         RouteDescription = r.RouteDescription,
                         NextDeparture = r.Schedules
                             .Where(s => s.DepartureTime > DateTime.Now)
                             .OrderBy(s => s.DepartureTime)
                             .Select(s => s.DepartureTime.ToString("yyyy-MM-dd HH:mm")) // 格式化日期為字串
                             .FirstOrDefault() ?? "目前無預定開航計畫" // 若無符合條件的行程則顯示此字串
                     })
                     .ToListAsync();


            return Ok(new
            {
                Data = shipments,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }









        [HttpGet("GetRouteImage/{routeId}")]
        public async Task<IActionResult> GetRouteImage(int routeId)
        {
            var image = await _context.RouteImages
                .Where(ri => ri.RouteId == routeId)
                .OrderBy(ri => ri.RouteImageId) // 取第一張圖片
                .Select(ri => ri.RouteImageUrl)
                .FirstOrDefaultAsync();

            if (image == null)
            {
                // 返回預設圖片的 URL
                return Ok(new { ImageUrl = "assets/img/Shipment/7.jpg" });
            }

            // 將 varbinary 轉換為 Base64 字串並加上 data URI 格式
            var base64Image = Convert.ToBase64String(image);
            var imageSrc = $"data:image/jpeg;base64,{base64Image}";

            return Ok(new { ImageUrl = imageSrc });
        }

        [HttpGet("{routeId}")]
        public async Task<ActionResult<ShipmentDetailViewModel>> GetShipmentDetail(int routeId)
        {
            try
            {
                var defaultMapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d14446.072087629718!2d121.36671011187296!3d25.1519807423538!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3442a45e3bf0f521%3A0x97688d97441a7ee1!2z5Y-w5YyX5riv!5e0!3m2!1szh-TW!2stw!4v1730807463318!5m2!1szh-TW!2stw";

                var shipmentDetail = await _context.Routes
                    .Include(r => r.OriginPort)
                    .Include(r => r.DestinationPort)
                    .Where(r => r.RouteId == routeId)
                    .Select(r => new ShipmentDetailViewModel
                    {
                        RouteId = r.RouteId,
                        OriginPortName = r.OriginPort.PortName,
                        DestinationPortName = r.DestinationPort.PortName,
                        Price = r.Price,
                        RouteDescription = r.RouteDescription,
                        OriginPort = new PortDetailViewModel
                        {
                            PortId = r.OriginPort.PortId,
                            PortName = r.OriginPort.PortName,
                            City = r.OriginPort.City,
                            CityDescription1 = r.OriginPort.CityDescription1,
                            CityDescription2 = r.OriginPort.CityDescription2,
                            PortGoogleMap = r.OriginPort.PortGoogleMap ?? defaultMapUrl
                        },
                        DestinationPort = new PortDetailViewModel
                        {
                            PortId = r.DestinationPort.PortId,
                            PortName = r.DestinationPort.PortName,
                            City = r.DestinationPort.City,
                            CityDescription1 = r.DestinationPort.CityDescription1,
                            CityDescription2 = r.DestinationPort.CityDescription2,
                            PortGoogleMap = r.DestinationPort.PortGoogleMap ?? defaultMapUrl
                        }
                    })
                    .FirstOrDefaultAsync();

                if (shipmentDetail == null)
                {
                    return NotFound();
                }

                return Ok(shipmentDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{routeId}/schedules")]
        public async Task<ActionResult<IEnumerable<ScheduleViewModel>>> GetSchedulesByRouteId(int routeId)
        {
            var schedules = await _context.Schedules
                .Where(s => s.RouteId == routeId)
                .Select(s => new ScheduleViewModel
                {
                    ScheduleId = s.ScheduleId,
                    DepartureTime = s.DepartureTime
                })
                .ToListAsync();

            if (schedules == null || !schedules.Any())
            {
                return NotFound();
            }

            return Ok(schedules);
        }

        [HttpGet("schedules/{scheduleId}")]
        public async Task<ActionResult<ScheduleViewModel>> GetScheduleById(int scheduleId)
        {
            var schedule = await _context.Schedules
                .Where(s => s.ScheduleId == scheduleId)
                .Select(s => new ScheduleViewModel
                {
                    ScheduleId = s.ScheduleId,
                    RouteId = s.RouteId,
                    DepartureTime = s.DepartureTime,
                    ArrivalTime = s.ArrivalTime,
                    Seats = s.Seats,
                    Capacity = s.Capacity
                })
                .FirstOrDefaultAsync();

            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateImage([FromForm] PortImageViewModel model)
        //{
        //    if (model.ImageFile != null && model.ImageFile.Length > 0)
        //    {
        //        // 先創建一個臨時 PortImage 物件，無圖片路徑
        //        var portImage = new PortImage
        //        {
        //            PortId = model.PortId,
        //            PortImageDescription = model.PortImageDescription
        //        };

        //        // 將該物件添加到資料庫並保存以獲取 PortImageId
        //        _context.PortImages.Add(portImage);
        //        await _context.SaveChangesAsync();

        //        // 使用獲取的 PortImageId 來創建唯一文件名稱
        //        var folderPath = Path.Combine("wwwroot", "images", "Shipment", "Port");
        //        if (!Directory.Exists(folderPath))
        //        {
        //            Directory.CreateDirectory(folderPath);
        //        }

        //        var uniqueFileName = $"{portImage.PortImageId}.jpg"; // 使用 PortImageId 作為文件名
        //        var filePath = Path.Combine(folderPath, uniqueFileName);

        //        // 保存圖片到指定文件夾
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await model.ImageFile.CopyToAsync(stream);
        //        }

        //        // 更新 PortImagePath 屬性，保存圖片的相對路徑到資料庫
        //        portImage.PortImageUrl = $"/images/Shipment/Port/{uniqueFileName}";
        //        _context.PortImages.Update(portImage);
        //        await _context.SaveChangesAsync();

        //        return RedirectToAction("Details", new { id = model.PortId });
        //    }

        //    ModelState.AddModelError("ImageFile", "Please upload a valid image file.");
        //    return View(model);
        //}

        [HttpGet("GetPortImages")]
        public async Task<IActionResult> GetPortImages(int portId)
        {
            var images = await _context.PortImages
                .Where(img => img.PortId == portId)
                .Select(img => new PortImageViewModel
                {
                    PortImageId = img.PortImageId,
                    PortId = img.PortId,
                    // 正確地使用 img 變數
                    PortImageUrl = Url.Content($"~/images/Shipment/Port/{img.PortImageId}.jpg"),
                    PortImageDescription = img.PortImageDescription
                }).ToListAsync();

            return Ok(images);
        }

        [HttpGet("GetPortIdByDestinationPort")]
        public async Task<IActionResult> GetPortIdByDestinationPort(string destinationPort)
        {
            var port = await _context.Ports
                .Where(p => p.PortName == destinationPort)
                .Select(p => p.PortId)
                .FirstOrDefaultAsync();

            if (port == 0)
            {
                return NotFound(new { message = "Port not found for the specified destination port." });
            }

            return Ok(new { PortId = port });
        }




    }
}
