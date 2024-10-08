using Microsoft.AspNetCore.Mvc;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class CouponController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
