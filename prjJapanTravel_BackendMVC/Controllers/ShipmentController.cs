using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult RDetail(int id)
        {
            // 查詢 Route 資料
            var routeData = (from route in _context.Routes
                             join originPort in _context.Ports on route.OriginPortId equals originPort.PortId
                             join destinationPort in _context.Ports on route.DestinationPortId equals destinationPort.PortId
                             where route.RouteId == id
                             select new RouteDetailViewModel
                             {
                                 RouteId = route.RouteId,
                                 OriginPortName = originPort.PortName,
                                 DestinationPortName = destinationPort.PortName,
                                 Price = route.Price,
                                 RouteDescription = route.RouteDescription
                             }).FirstOrDefault(); // 確保只拿一筆 Route 資料

            // 查詢 Schedule 資料
            var schedules = (from schedule in _context.Schedules
                             where schedule.RouteId == id
                             select schedule).ToList();

            // 確保兩個資料來源都不為 null
            if (routeData == null)
            {
                // 處理沒有資料的情況，可以顯示錯誤訊息或跳轉到其他頁面
                return NotFound($"未找到 RouteId {id} 的資料");
            }

            // 將 Schedules 加入 ViewModel
            routeData.Schedules = schedules;

            // 傳遞 ViewModel 到 View
            return View(routeData);
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


    }
}
