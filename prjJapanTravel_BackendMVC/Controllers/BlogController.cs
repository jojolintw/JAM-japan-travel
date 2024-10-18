using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.AdminViewModels;
using prjJapanTravel_BackendMVC.ViewModels.BlogViewModels;
using prjJapanTravel_BackendMVC.ViewModels.MemberViewModels;
using prjJapanTravel_BackendMVC.ViewModels.OrderViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class BlogController : Controller
    {
        public JapanTravelContext _context;

        public BlogController(JapanTravelContext context)
        {
            _context = context;
        }
        public IActionResult List(AKeywordViewModel avm)
        {
            string keyword = avm.txtKeyword;

            var datas = _context.ArticleMains
       .Include(m => m.ArticleHashtags)
       .ThenInclude(h => h.HashtagNumberNavigation)
       .AsQueryable(); // 使用 IQueryable 以便后续可以动态添加条件

            if (!string.IsNullOrEmpty(keyword))
            {
                datas = datas.Where(p =>
                    p.ArticleTitle.Contains(keyword) ||
                    p.ArticleContent.Contains(keyword) ||
                    p.ArticleStatusnumberNavigation.StatusName.Contains(keyword));
            }



            var articledatas = datas.Select(m => new ArticleMainViewModel
            {
                文章編號 = m.ArticleNumber,
                會員編號 = m.MemberId,
                文章發布時間 = m.ArticleLaunchtime.ToDateTime(TimeOnly.MinValue), // 确保 DateOnly 转换正确
                文章狀態 = m.ArticleStatusnumberNavigation.StatusName,
                文章標題 = m.ArticleTitle,
                文章最後更新時間 = m.ArticleUpdatetime.ToDateTime(TimeOnly.MinValue), // 确保 DateOnly 转换正确
                文章內容 = m.ArticleContent,
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
            ViewBag.HashtagList = new SelectList(_context.ArticleStatuses.ToList(), "StatusNumber", "StatusName");

            return View();
        }
        [HttpPost]
        public IActionResult Create(ArticleMain am)
        {
            am.ArticleLaunchtime = DateOnly.FromDateTime(DateTime.Now);
            am.ArticleUpdatetime = DateOnly.FromDateTime(DateTime.Now);
            _context.ArticleMains.Add(am);
            _context.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Delete(int? id)
        {
            var data = _context.ArticleMains.FirstOrDefault(am => am.ArticleNumber == id);
            if (data != null)
            {
                _context.ArticleMains.Remove(data);
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }
        public IActionResult Edit(int? id)
        {

            var data = _context.ArticleMains
                .Where(i => i.ArticleNumber == id)
                .Select(i => new ArticleMainViewModel()
                {
                    文章編號=i.ArticleNumber,
                    會員編號 =i.MemberId,
                    文章狀態 =i.ArticleStatusnumberNavigation.StatusName,
                    文章標題=i.ArticleTitle,
                    //文章內容=i.ArticleContent,

                }).FirstOrDefault();

            ViewBag.HashtagList = new SelectList(_context.ArticleStatuses.ToList()
                , "StatusNumber", "StatusName",data.文章狀態);

            return View(data);
        }
        //[HttpPost]
        //public IActionResult Edit(ArticleMainViewModel amvm)
        //{
        //    amvm.ArticleUpdatetime = DateOnly.FromDateTime(DateTime.Now);

        //}

    }
}
