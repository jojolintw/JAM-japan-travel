using Microsoft.AspNetCore.Mvc;
using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class AdminAPIController : Controller
    {
        public JapanTravelContext _context;

        public AdminAPIController(JapanTravelContext context) 
        {
            _context=context;
        }


        [HttpGet]
        public IActionResult GetData(int adminId)
        {
            Admin ad = _context.Admins.FirstOrDefault(a => a.AdminId == adminId);
            return Json(ad);
        }
    }
}
