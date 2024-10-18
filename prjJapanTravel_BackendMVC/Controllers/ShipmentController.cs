using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.ShipmentViewModels;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly JapanTravelContext _context;

        // 在構造函數中注入 JapanTravelContext
        public ShipmentController(JapanTravelContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = from route in _context.Routes
                       join originPort in _context.Ports on route.OriginPortId equals originPort.PortId
                       join destinationPort in _context.Ports on route.DestinationPortId equals destinationPort.PortId
                       select new
                       {
                           route.RouteId,
                           OriginPortName = originPort.PortName,
                           DestinationPortName = destinationPort.PortName,
                           route.Price,
                           route.RouteDescription
                       };

            return View(data.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.OriginPortList = new SelectList(_context.Ports.ToList(), "PortId", "PortName");
            ViewBag.DestinationPortList = new SelectList(_context.Ports.ToList(), "PortId", "PortName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Models.Route r)
        {
            if (ModelState.IsValid)
            {
                _context.Routes.Add(r);  // 新增資料
                _context.SaveChanges(); // 儲存變更
                return RedirectToAction("Index");  // 回到列表頁面
            }
            ViewBag.OriginPortList = new SelectList(_context.Ports.ToList(), "PortId", "PortName");
            ViewBag.DestinationPortList = new SelectList(_context.Ports.ToList(), "PortId", "PortName");
            return View(r); // 回傳到視圖並顯示錯誤
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); // 返回 404 找不到頁面
            }

            var route = _context.Routes.FirstOrDefault(r => r.RouteId == id);

            if (route == null)
            {
                return NotFound(); // 如果找不到路由，返回 404
            }

            // 刪除路由
            _context.Routes.Remove(route);
            _context.SaveChanges(); // 儲存變更到資料庫

            return RedirectToAction("Index"); // 刪除後返回列表頁面
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // 返回 404 找不到頁面
            }

            // 根據 id 查詢對應的 Route
            var route = _context.Routes.FirstOrDefault(r => r.RouteId == id);

            if (route == null)
            {
                return NotFound(); // 如果找不到對應的 Route，返回 404
            }

            // 將出發港口和目的地港口的選項列表傳給視圖
            ViewBag.OriginPortList = new SelectList(_context.Ports.ToList(), "PortId", "PortName", route.OriginPortId);
            ViewBag.DestinationPortList = new SelectList(_context.Ports.ToList(), "PortId", "PortName", route.DestinationPortId);

            return View(route); // 返回編輯頁面，並將 route 資料傳遞給視圖
        }
        [HttpPost]
        public ActionResult Edit(int id, Models.Route updatedRoute)
        {
            if (id != updatedRoute.RouteId)
            {
                return NotFound(); // 如果路徑中的 id 和提交表單中的 RouteId 不匹配，返回 404
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 更新資料庫中的資料
                    _context.Update(updatedRoute);
                    _context.SaveChanges(); // 儲存變更到資料庫
                }
                catch (Exception ex)
                {
                    // 如果發生例外，可以記錄例外或顯示錯誤信息
                    ModelState.AddModelError("", "更新失敗: " + ex.Message);
                    return View(updatedRoute); // 返回編輯頁面，並顯示錯誤信息
                }

                // 更新成功後，重定向回到列表頁面
                return RedirectToAction("Index");
            }

            // 如果 ModelState 無效，則重新顯示編輯頁面
            return View(updatedRoute);
        }
        //--------------------------------------------------------------------------
        [Route("Shipment/{routeId}/RDetail")]
        public IActionResult RDetail(int routeId)
        {
            // 查詢 Route 資料
            var routeData = (from route in _context.Routes
                             join originPort in _context.Ports on route.OriginPortId equals originPort.PortId
                             join destinationPort in _context.Ports on route.DestinationPortId equals destinationPort.PortId
                             where route.RouteId == routeId
                             select new RouteDetailViewModel
                             {
                                 RouteId = route.RouteId,
                                 OriginPortName = originPort.PortName,
                                 DestinationPortName = destinationPort.PortName,
                                 Price = route.Price,
                                 RouteDescription = route.RouteDescription,
                                 Images = (from img in _context.RouteImages
                                           where img.RouteId == route.RouteId
                                           select img.Image).ToList(), // 加載多張圖片
                                 ImageDescriptions = (from img in _context.RouteImages
                                                      where img.RouteId == route.RouteId
                                                      select img.Description).ToList() // 加載圖片描述
                             }).FirstOrDefault();

            if (routeData == null)
            {
                return NotFound();
            }

            return View(routeData);
        }

        [HttpGet]
        [Route("Shipment/{routeId}/SCreate")]
        public IActionResult SCreate(int routeId)
        {
            ViewBag.RouteId = routeId; // 傳遞 RouteId 給視圖

            return View();
        }

        [HttpPost]
        [Route("Shipment/{routeId}/SCreate")]
        public IActionResult SCreate(int routeId, Schedule newSchedule)
        {
            if (ModelState.IsValid)
            {
                newSchedule.RouteId = routeId; // 設定 RouteId
                _context.Schedules.Add(newSchedule);
                _context.SaveChanges();

                return RedirectToAction("RDetail", new { routeId = routeId }); // 新增成功後，返回 Route 詳細頁
            }

            return View(newSchedule); // 如果驗證失敗，重新顯示新增頁面
        }

        [HttpGet]
        [Route("Shipment/{routeId}/SEdit/{imageId}")]
        public IActionResult SEdit(int routeId, int imageId)
        {
            var image = _context.RouteImages.FirstOrDefault(img => img.RouteImageId == imageId);
            if (image == null)
            {
                return NotFound(); // 找不到要編輯的圖片
            }

            var model = new RouteImageEditViewModel
            {
                RouteImageId = image.RouteImageId,
                RouteId = image.RouteId,
                Image = image.RouteImage1,
                Description = image.RouteImageDescription
            };

            return View(model);
        }

        [HttpPost]
        [Route("Shipment/{routeId}/SEdit/{imageId}")]
        public IActionResult SEdit(int routeId, RouteImageEditViewModel model, IFormFile newImage)
        {
            if (ModelState.IsValid)
            {
                var image = _context.RouteImages.FirstOrDefault(img => img.RouteImageId == model.RouteImageId);
                if (image == null)
                {
                    return NotFound(); // 找不到要編輯的圖片
                }

                // 更新圖片描述
                image.RouteImageDescription = model.Description;

                // 檢查是否上傳了新圖片
                if (newImage != null && newImage.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        newImage.CopyTo(memoryStream);
                        image.RouteImage1 = memoryStream.ToArray(); // 將新圖片轉換為 byte[]
                    }
                }

                _context.RouteImages.Update(image); // 更新資料庫中的圖片
                _context.SaveChanges(); // 保存變更

                return RedirectToAction("RDetail", new { routeId = routeId }); // 重定向回詳情頁面
            }

            return View(model); // 如果模型不合法，返回原始視圖
        }

        

        public ActionResult SDelete(int? id)
        {
            if (id == null)
            {
                return NotFound(); // 返回 404 找不到頁面
            }

            // 正確地使用 ScheduleId 來查詢
            var sch = _context.Schedules.FirstOrDefault(s => s.ScheduleId == id);

            if (sch == null)
            {
                return NotFound(); // 如果找不到對應的 Schedule，返回 404
            }

            // 刪除 Schedule
            _context.Schedules.Remove(sch);
            _context.SaveChanges(); // 儲存變更到資料庫

            // 刪除後重定向回到該 Route 的詳細頁
            return RedirectToAction("RDetail", new { routeId = sch.RouteId }); // 使用 routeId
        }



        //--------------------------------------------------------------------------

        public IActionResult ICreate(int routeId, int? routeImageId)
        {
            // 傳遞 RouteId 到 ViewBag 或直接使用 ViewModel
            ViewBag.RouteId = routeId;

            // 如果需要初始化其他數據，可以在這裡做
            var model = new RouteDetailViewModel
            {
                RouteId = routeId,
                // 其他初始化資料
            };

            return View(model);
        }

        [HttpPost]
        [Route("Shipment/{routeId}/ICreate")]
        public IActionResult ICreate(int routeId, RouteDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 檢查是否有檔案上傳
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        // 將檔案複製到記憶體流
                        model.ImageFile.CopyTo(memoryStream);
                        model.Image = memoryStream.ToArray(); // 將記憶體流轉換為 byte[]
                    }

                // 儲存圖片到資料庫
                var routeImage = new RouteImage
                {
                    RouteId = model.RouteId,
                    Image = model.Image,
                    Description = model.ImageDescription
                };

                _context.RouteImages.Add(routeImage);
                _context.SaveChanges();

                return RedirectToAction("RDetail", new { id = model.RouteId });
            }

            // 如果模型驗證失敗，返回新增頁面並顯示錯誤
            return View(model);
        }



    }
}
