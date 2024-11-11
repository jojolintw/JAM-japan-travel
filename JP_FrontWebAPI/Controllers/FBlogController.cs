using JP_FrontWebAPI.DTOs.Blog;
using JP_FrontWebAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using static JP_FrontWebAPI.DTOs.Blog.ArticleDTO;

namespace JP_FrontWebAPI.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]")]
    [ApiController]
    public class FBlogController : ControllerBase
    {

        private JapanTravelContext _context;
        public FBlogController(JapanTravelContext context)
        {
            _context = context;
        }

        // 取得所有文章
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticles()
        {

            var articles = await _context.ArticleMains
        .Include(a => a.ArticleHashtags) // 确保加载 ArticleHashtags
        .ThenInclude(h => h.HashtagNumberNavigation) // 加载 HashtagMain
        .ToListAsync();

            var articleDtos = articles.Select(a => new ArticleDTO
            {
                ArticleId = a.ArticleNumber,
                MemberId = a.MemberId,
                LaunchTime = a.ArticleLaunchtime,
                ArticleTitle = a.ArticleTitle,
                LastUpdateTime = a.ArticleLastupatetime,
                ArticleContent = a.ArticleContent,
                ArticleHashtags = a.ArticleHashtags.Select(h => h.HashtagNumberNavigation.HashtagName).ToList() // 获取标签名称
            }).ToList();

            return Ok(articleDtos);
        }

        //根據id獲取文章
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDTO>> GetArticleById(int id)
        {
            //int a = GetMemberId();

            var article = await _context.ArticleMains
        .Include(a => a.ArticleHashtags) // 加载 ArticleHashtags
        .ThenInclude(h => h.HashtagNumberNavigation) // 加载 HashtagMain
        .FirstOrDefaultAsync(a => a.ArticleNumber == id); // 使用 FirstOrDefaultAsync

            if (article == null)
            {
                return NotFound();
            }

            var articleDto = new ArticleDTO
            {
                ArticleId = article.ArticleNumber,
                MemberId = article.MemberId,
                LaunchTime = article.ArticleLaunchtime,
                ArticleTitle = article.ArticleTitle,
                LastUpdateTime = article.ArticleLastupatetime,
                ArticleContent = article.ArticleContent,
                ArticleHashtags = article.ArticleHashtags
                    .Select(h => h.HashtagNumberNavigation.HashtagName) // 提取标签名称
                    .ToList()
            };

            return Ok(articleDto);
        }
        // 搜索文章（根據標題和內容搜索）
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> SearchArticles(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return BadRequest("Keyword cannot be empty.");
            }

            var articles = await _context.ArticleMains
            .Where(a => a.ArticleTitle.Contains(keyword) || a.ArticleContent.Contains(keyword)) // 查找标题或内容包含关键字的文章
            .Include(a => a.ArticleHashtags) // 关联查询 ArticleHashtags
            .ThenInclude(h => h.HashtagNumberNavigation) // 进一步关联 Hashtag
            .ToListAsync(); // 异步查询



            var articleDtos = articles.Select(a => new ArticleDTO
            {
                ArticleId = a.ArticleNumber,
                MemberId = a.MemberId,
                LaunchTime = a.ArticleLaunchtime,
                ArticleTitle = a.ArticleTitle,
                LastUpdateTime = a.ArticleLastupatetime,
                ArticleContent = a.ArticleContent,
                ArticleHashtags = a.ArticleHashtags
                    .Select(h => h.HashtagNumberNavigation.HashtagName)
                    .ToList()
            }).ToList();

            return Ok(articleDtos);
        }

        [HttpGet("hashtags")]
        public async Task<ActionResult<IEnumerable<string>>> GetHashtags()
        {
            var hashtags = await _context.HashtagMains
                .Select(h => h.HashtagName)
                .Distinct()
                .ToListAsync();

            return Ok(hashtags);
        }
        //獲取某個標籤相關的文章
        [HttpGet("articles-by-hashtag")]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticlesByHashtag(string hashtag)

        {
            if (string.IsNullOrEmpty(hashtag))
            {
                return BadRequest("Hashtag cannot be empty.");
            }

            var articles = await _context.ArticleMains
                .Where(a => a.ArticleHashtags.Any(ah => ah.HashtagNumberNavigation.HashtagName == hashtag))
                .Include(a => a.ArticleHashtags)
                .ThenInclude(ah => ah.HashtagNumberNavigation)
                .ToListAsync();

            var articleDtos = articles.Select(a => new ArticleDTO
            {
                ArticleId = a.ArticleNumber,
                MemberId = a.MemberId,
                LaunchTime = a.ArticleLaunchtime,
                ArticleTitle = a.ArticleTitle,
                LastUpdateTime = a.ArticleLastupatetime,
                ArticleContent = a.ArticleContent,
                ArticleHashtags = a.ArticleHashtags
                    .Select(ah => ah.HashtagNumberNavigation.HashtagName)
                    .ToList()
            }).ToList();

            return Ok(articleDtos);
        }

        // 假设这是一个控制器方法，用于删除指定的文章
        [HttpDelete("{articleNumber}")]
        public async Task<IActionResult> DeleteArticle(int articleNumber)
        {
            // 查找要删除的文章
            var article = await _context.ArticleMains.FindAsync(articleNumber);

            // 如果文章不存在，返回 404 Not Found
            if (article == null)
            {
                return NotFound($"Article with number {articleNumber} not found.");
            }

            // 删除与该文章相关的记录（如果有外键约束需要删除）
            var articleHashtags = _context.ArticleHashtags.Where(a => a.ArticleNumber == articleNumber);
            _context.ArticleHashtags.RemoveRange(articleHashtags);

            // 删除文章本身
            _context.ArticleMains.Remove(article);

            // 保存更改到数据库
            await _context.SaveChangesAsync();

            // 返回 204 No Content，表示删除成功
            return NoContent();
        }


        //private int GetMemberId()
        //{



        // 1.抓到使用者
        //var email = User.Claims.FirstOrDefault(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

        //_context.Members  找=>id
        // 比對MemberId

        // 2.判定是否有修改權限

        //return 1;
        //}

        // POST: api/FBlog
        [HttpPost]
        public async Task<ActionResult<ArticleDTO>> CreateArticle([FromBody] ArticleCreateDTO articleCreateDTO)
        {
            // 檢查輸入的資料是否有效
            if (articleCreateDTO == null || string.IsNullOrEmpty(articleCreateDTO.ArticleTitle) || string.IsNullOrEmpty(articleCreateDTO.ArticleContent))
            {
                return BadRequest("Invalid article data.");
            }

            // 1. 設置預設狀態為 1
            var statusNumber = 1; // 預設狀態為 1


            // 1. 創建一個新的 ArticleMain 物件
            var articleMain = new ArticleMain
            {
                MemberId = articleCreateDTO.MemberId,
                ArticleTitle = articleCreateDTO.ArticleTitle,
                ArticleContent = articleCreateDTO.ArticleContent,
                ArticleLaunchtime = DateTime.Now,  // 設置發佈時間為當前時間
                ArticleLastupatetime = DateTime.Now,  // 設置最後更新時間為當前時間
                ArticleStatusnumber = statusNumber  // 設置為預設的狀態 1
            };

            // 2. 處理標籤關聯
            var hashtags = new List<ArticleHashtag>();

            foreach (var hashtagNumber in articleCreateDTO.HashtagNumbers)
            {
                // 查詢標籤是否存在
                var hashtag = await _context.HashtagMains.FindAsync(hashtagNumber);
                if (hashtag != null)
                {
                    // 創建標籤與文章的關聯
                    hashtags.Add(new ArticleHashtag
                    {
                        ArticleNumber = articleMain.ArticleNumber,  // 這裡會在儲存後自動填充
                        HashtagNumber = hashtag.HashtagNumber
                    });
                }
                else
                {
                    // 如果標籤不存在，返回 BadRequest
                    return BadRequest($"Hashtag with ID {hashtagNumber} does not exist.");
                }
            }

            // 3. 設置標籤關聯到文章
            articleMain.ArticleHashtags = hashtags;

            // 4. 儲存文章
            _context.ArticleMains.Add(articleMain);
            await _context.SaveChangesAsync();

            // 5. 返回創建的文章資料（包含標籤名稱）
            var articleDTO = new ArticleDTO
            {
                ArticleId = articleMain.ArticleNumber, // 記得使用 ArticleNumber 作為 ArticleId
                MemberId = articleMain.MemberId,
                LaunchTime = articleMain.ArticleLaunchtime,
                LastUpdateTime = articleMain.ArticleLastupatetime,
                ArticleTitle = articleMain.ArticleTitle,
                ArticleContent = articleMain.ArticleContent,
                ArticleHashtags = articleMain.ArticleHashtags
                    .Select(h => h.HashtagNumberNavigation.HashtagName)  // 提取標籤名稱
                    .ToList()
            };

            // 6. 回傳創建成功的文章資料
            return CreatedAtAction(nameof(GetArticleById), new { id = articleMain.ArticleNumber }, articleDTO);
        }
    }
}

