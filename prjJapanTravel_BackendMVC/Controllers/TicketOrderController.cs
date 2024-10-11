using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class TicketOrderController : Controller
    {
        public JapanTravelContext _context;
        public TicketOrderController(JapanTravelContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            var datas = _context.TicketOrders.Select(m => m);
            return View(datas);
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
