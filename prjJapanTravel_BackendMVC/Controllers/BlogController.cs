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
                文章發布時間 = (m.ArticleLaunchtime).ToDateTime(TimeOnly.MinValue),  //這個錯誤信息表明，在將 System.DateOnly 物件轉換為 DateTime 時發生了無法轉換的異常。這通常發生在你嘗試將 DateOnly 物件傳遞給 Convert.ToDateTime 方法時。
                文章狀態編號 = m.ArticleStatusnumber,
                文章標題 = m.ArticleTitle,
                文章更新時間 = (m.ArticleUpdatetime).ToDateTime(TimeOnly.MinValue),
                文章內容=m.ArticleContent,
            });
            return View(articledatas);
        }
    }
}
