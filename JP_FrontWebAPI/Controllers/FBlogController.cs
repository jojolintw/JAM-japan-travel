using JP_FrontWebAPI.DTOs.Blog;
using JP_FrontWebAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        // 搜索文章（根据标题进行搜索）
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
    }
}

