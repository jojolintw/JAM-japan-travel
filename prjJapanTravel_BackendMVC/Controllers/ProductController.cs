using Microsoft.AspNetCore.Mvc;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
