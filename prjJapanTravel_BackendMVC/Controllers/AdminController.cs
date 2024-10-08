using Microsoft.AspNetCore.Mvc;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
