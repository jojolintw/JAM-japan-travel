using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class AdminController : Controller
    {
        public JapanTravelContext _context;

        public AdminController(JapanTravelContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            var admins = _context.Admins.Select(a => a);
            return View(admins);
        }
    }
}
