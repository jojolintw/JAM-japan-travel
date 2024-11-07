using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels;
using prjJapanTravel_BackendMVC.ViewModels.ShipmentViewModels;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class PortsController : Controller
    {
        private readonly JapanTravelContext _context;

        public PortsController(JapanTravelContext context)
        {
            _context = context;
        }

        // GET: Ports
        public async Task<IActionResult> Index()
        {
            var ports = await _context.Ports.ToListAsync();
            return View(ports);
        }

        // GET: Ports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PortName, City, CityDescription1, CityDescription2, PortGoogleMap")] Port port)
        {
            if (ModelState.IsValid)
            {
                _context.Add(port);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(port);
        }

    }
}
