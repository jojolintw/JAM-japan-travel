using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using prjJapanTravel_BackendMVC.Models;

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
        [HttpPost]
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


    }
}
