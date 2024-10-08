using Microsoft.AspNetCore.Mvc;
using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class MemberController : Controller
    {
        public JapanTravelContext _context;

        public MemberController(JapanTravelContext context) 
        {
            _context = context;
        }

        public IActionResult List()
        {
            var memberdatas = _context.Members.Select(m =>m);
            return View(memberdatas);
        }
    }
}
