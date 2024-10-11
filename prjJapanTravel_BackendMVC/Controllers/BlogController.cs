using Microsoft.AspNetCore.Mvc;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.BlogViewModels;
using prjJapanTravel_BackendMVC.ViewModels.MemberViewModels;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class BlogController : Controller
    {
        public JapanTravelContext _context;

        public BlogController(JapanTravelContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            var articledatas = _context.ArticleMains.Select(m => new ArticleMainViewModel
            {
                文章編號 = m.ArticleNumber,
                會員編號 = m.MemberId,
                文章發布時間 = Convert.ToDateTime(m.ArticleLaunchtime),
                文章狀態編號 = m.ArticleStatusnumber,
                文章標題 = m.ArticleTitle,
                文章更新時間 = Convert.ToDateTime(m.ArticleUpdatetime),

            });
            return View(articledatas);
        }
    }
}
