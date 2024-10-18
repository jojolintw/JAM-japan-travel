using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels;
using prjJapanTravel_BackendMVC.ViewModels.ShipmentViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class ShipmentController : Controller
    {
        public JapanTravelContext _context;
        public ShipmentController(JapanTravelContext context)
        {
            _context = context;
        }
        //---------------------首頁------------------------------------------
        public async Task<IActionResult> Index()
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

            return View(shipments);
        }
        //---------------------Create------------------------------------------
        public async Task<IActionResult> Create()
        {
            var ports = await _context.Ports.ToListAsync(); // 取得所有港口以填充下拉選單
            ViewBag.Ports = ports;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Route route)
        {
            if (ModelState.IsValid)
            {
                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var ports = await _context.Ports.ToListAsync(); // 取得所有港口以填充下拉選單
            ViewBag.Ports = ports;
            return View(route);
        }
        //---------------------Edit------------------------------------------
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }

            var ports = await _context.Ports.ToListAsync(); // 取得所有港口以填充下拉選單
            ViewBag.Ports = ports;
            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Route route)
        {
            if (id != route.RouteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(route);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(route.RouteId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var ports = await _context.Ports.ToListAsync(); // 取得所有港口以填充下拉選單
            ViewBag.Ports = ports;
            return View(route);
        }

        private bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.RouteId == id);
        }
        //---------------------Delete------------------------------------------
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var route = await _context.Routes.FindAsync(id);
            if (route != null)
            {
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        //---------------------Details------------------------------------------

        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.OriginPort)
                .Include(r => r.DestinationPort)
                .Include(r => r.RouteImages) // 包含圖片
                .FirstOrDefaultAsync(r => r.RouteId == id);

            if (route == null)
            {
                return NotFound();
            }

            var schedules = await _context.Schedules
                .Where(s => s.RouteId == id)
                .ToListAsync();

            var viewModel = new RouteDetailsViewModel
            {
                Route = route,
                Schedules = schedules,
                RouteImages = route.RouteImages // 添加圖片資料到 ViewModel
            };

            return View(viewModel);
        }
        // 顯示所有航線圖片
        public async Task<IActionResult> RouteImages(int routeId)
        {
            var routeImages = await _context.RouteImages
                                             .Where(r => r.RouteId == routeId)
                                             .ToListAsync();
            ViewBag.RouteId = routeId; // 傳遞 RouteId 以便於返回
            return View(routeImages);
        }

        // 顯示所有排班資訊
        public async Task<IActionResult> Schedules(int routeId)
        {
            var schedules = await _context.Schedules
                                           .Where(s => s.RouteId == routeId)
                                           .ToListAsync();
            ViewBag.RouteId = routeId; // 傳遞 RouteId 以便於返回
            return View(schedules);
        }

        // 刪除排班
        [HttpPost]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", new { id = schedule.RouteId }); // 返回 Details 頁面
        }
        // 新增排班
        [HttpGet]
        public IActionResult CreateSchedule(int routeId)
        {
            var schedule = new Schedule { RouteId = routeId };
            return View(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction("Schedules", new { routeId = schedule.RouteId }); // 返回到 Schedules 頁面
            }
            return View(schedule);
        }

        // 編輯排班
        [HttpGet]
        public async Task<IActionResult> EditSchedule(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null) return NotFound();
            return View(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> EditSchedule(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Schedules.Update(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction("Schedules", new { routeId = schedule.RouteId }); // 返回到 Schedules 頁面
            }
            return View(schedule);
        }



    }
}
