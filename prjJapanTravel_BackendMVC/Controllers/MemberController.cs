using Microsoft.AspNetCore.Mvc;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
