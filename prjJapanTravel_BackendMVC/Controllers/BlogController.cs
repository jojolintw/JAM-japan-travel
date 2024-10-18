using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult List()
        {
            var articledatas = _context.ArticleMains
                .Include(m => m.ArticleHashtags)
                .ThenInclude(h => h.HashtagNumberNavigation).Select(m => new ArticleMainViewModel
            {
                文章編號 = m.ArticleNumber,
                會員編號 = m.MemberId,
                文章發布時間 = (m.ArticleLaunchtime).ToDateTime(TimeOnly.MinValue),  //這個錯誤信息表明，在將 System.DateOnly 物件轉換為 DateTime 時發生了無法轉換的異常。這通常發生在你嘗試將 DateOnly 物件傳遞給 Convert.ToDateTime 方法時。
                文章狀態 = m.ArticleStatusnumberNavigation.StatusName,
                文章標題 = m.ArticleTitle,
                文章最後更新時間 = (m.ArticleUpdatetime).ToDateTime(TimeOnly.MinValue),  //DateTime = DateOnly
                文章內容 =m.ArticleContent,
                文章使用的Hashtag = GetHashtags(m.ArticleHashtags),
            });
            return View(articledatas);
        }

        private static string GetHashtags(ICollection<ArticleHashtag> hashtags)
        {
            return string.Join(", ", hashtags.Select(h => h.HashtagNumberNavigation.HashtagName));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ArticleMain am)
        {
            _context.ArticleMains.Add(am);
            _context.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
