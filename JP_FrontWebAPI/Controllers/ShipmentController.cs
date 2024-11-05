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
        public async Task<ActionResult<IEnumerable<ShipmentListViewModel>>> GetShipments()
        {
            var shipments = await _context.Routes
                .Include(r => r.OriginPort)
                .Include(r => r.DestinationPort)
                .Select(r => new ShipmentListViewModel
                {
                    RouteId = r.RouteId,
                    OriginPortName = r.OriginPort.PortName,
                    DestinationPortName = r.DestinationPort.PortName,
                    Price = r.Price,
                    RouteDescription = r.RouteDescription
                })
                .ToListAsync();

            return Ok(shipments);
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
                        PortGoogleMap = r.OriginPort.PortGoogleMap
                    },
                    DestinationPort = new PortDetailViewModel
                    {
                        PortId = r.DestinationPort.PortId,
                        PortName = r.DestinationPort.PortName,
                        City = r.DestinationPort.City,
                        CityDescription1 = r.DestinationPort.CityDescription1,
                        CityDescription2 = r.DestinationPort.CityDescription2,
                        PortGoogleMap = r.DestinationPort.PortGoogleMap
                    }
                })
                .FirstOrDefaultAsync();

            if (shipmentDetail == null)
            {
                return NotFound();
            }

            return Ok(shipmentDetail);
        }



    }
}
