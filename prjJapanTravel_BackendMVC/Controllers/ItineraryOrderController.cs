using Microsoft.AspNetCore.Mvc;
using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class ItineraryOrderController : Controller
    {
        public JapanTravelContext _context;
        public ItineraryOrderController(JapanTravelContext context)
        {
            _context = context;
        }

        public IActionResult List()
        {
            var datas = _context.ItineraryOrders.Select(m => m);
            return View(datas);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int? id)
        {
            return View();
        }

        public IActionResult Cancel(int? id)
        {
            return View();
        }
    }
}
