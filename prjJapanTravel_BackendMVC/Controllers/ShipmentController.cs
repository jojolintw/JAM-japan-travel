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



    }
}
