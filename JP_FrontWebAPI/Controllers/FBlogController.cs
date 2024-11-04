using JP_FrontWebAPI.DTOs.Blog;
using JP_FrontWebAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static JP_FrontWebAPI.DTOs.Blog.articleDTO;

namespace JP_FrontWebAPI.Controllers
{
    [EnableCors("All")]
    [Route("api/[controller]")]
    [ApiController]
    public class FBlogController : ControllerBase
    {
        
            private  JapanTravelContext _context;
            public FBlogController(JapanTravelContext context)
            {
                _context = context;
            }

            // 取得所有文章
            [HttpGet]
            public async Task<ActionResult<IEnumerable<articleDTO>>> GetArticles()
            {
            //return await _context.ArticleMains.ToListAsync();

            var articles = await _context.ArticleMains
        .Include(a => a.ArticleHashtags) // 确保加载 ArticleHashtags
        .ThenInclude(h => h.HashtagNumberNavigation) // 加载 HashtagMain
        .ToListAsync();

            var articleDtos = articles.Select(a => new ArticleDto
            {
                ArticleId = a.ArticleNumber,
                MemberId = a.MemberId,
                LaunchTime = a.ArticleLaunchtime,
                ArticleTitle = a.ArticleTitle,
                LastUpdateTime = a.ArticleLastupatetime,
                ArticleContent = a.ArticleContent,
                ArticleHashtag = a.ArticleHashtags.Select(h => h.HashtagNumberNavigation.HashtagName).ToList() // 获取标签名称
            }).ToList();

            return Ok(articleDtos);
        }
            // 根據區域行程
            //[HttpGet("region/{region}")]
            //public async Task<ActionResult<IEnumerable<ArticleMain>>> GetItinerariesByRegion(int region)
            //{
            //    var itineraries = await _context.Itineraries.Where(i => i.Region == region).ToListAsync();
            //    return itineraries;
            //}

            // 根據 ID 獲取行程
            //[HttpGet("{id}")]
            //public async Task<ActionResult<ArticleMain>> GetItineraryById(int id)
            //{
            //    var itinerary = await _context.Itineraries.FindAsync(id);
            //    if (itinerary == null)
            //    {
            //        return NotFound();
            //    }
            //    return itinerary;

        }
    }

