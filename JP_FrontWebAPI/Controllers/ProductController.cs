using Microsoft.AspNetCore.Mvc;
using JP_FrontWebAPI.DTOs.Itinerary;
using JP_FrontWebAPI.DTOs.Shared;
using JP_FrontWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JP_FrontWebAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("All")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private JapanTravelContext _JP;
        IWebHostEnvironment _enviroment;

        public ProductController(JapanTravelContext context, IWebHostEnvironment environment)
        {
            _JP = context;
            _enviroment = environment;
        }

        [HttpGet("activity")]
        public ActionResult<IEnumerable<Activity>> GetActivityNames()
        {
            var activity = _JP.Activities
                .Select(a => new Activity
                {
                    ActivitySystemId = a.ActivitySystemId,
                    ActivityName = a.ActivityName
                })
                .Distinct()
                .ToList();

            return Ok(activity);
        }

        [HttpGet("list")]
        public ActionResult<IEnumerable<Itinerary_List>> GetList()
        {
            var datas = _JP.Itineraries.Select(n => new Itinerary_List
            {
                ItinerarySystemId = n.ItinerarySystemId,
                ItineraryName = n.ItineraryName,
                AreaName = n.AreaSystem != null ? n.AreaSystem.AreaName : "",
                DepartureDate = n.ItineraryDates.Select(d => d.DepartureDate).ToList(),
                ActivityId = n.ActivitySystem.ActivitySystemId,
                AvailableDate = n.Available,
                Price = n.Price,
                ImagePath = n.Images.Where(img => img.ItinerarySystemId == n.ItinerarySystemId)
                                    .Select(i => i.ImagePath)
                                    .FirstOrDefault()
            }).ToList();
            return Ok(datas);
        }

        [HttpGet("detail/{id}")]
        public ActionResult<Itineray_Detail> GetItineraryById(int id)
        {
            var data = _JP.Itineraries.Where(n => n.ItinerarySystemId == id)
                .Select(n => new Itineray_Detail
                {
                    ItinerarySystemId = n.ItinerarySystemId,
                    ItineraryName = n.ItineraryName,
                    ActivityName = n.ActivitySystem.ActivityName,
                    ActivityId = n.ActivitySystem.ActivitySystemId,
                    Price = (decimal)n.Price,
                    AreaName = n.AreaSystem.AreaName,
                    ItineraryBatch = n.ItineraryDates.Where(batch => batch.ItinerarySystemId == n.ItinerarySystemId)
                                                     .Select(d => new Itinerary_Date
                                                     {
                                                         ItineraryDateSystemId = d.ItineraryDateSystemId,
                                                         DepartureDate = d.DepartureDate,
                                                         Stock = d.Stock
                                                     }).ToList(),
                    ImagePath = n.Images.Where(i => i.ItinerarySystemId == n.ItinerarySystemId)
                                        .Select(i => i.ImagePath).ToList(),
                    ItineraryDetail = n.ItineraryDetail,
                    ItineraryBrief = n.ItineraryBrief,
                    ItineraryNote = n.ItineraryNotes
                }).FirstOrDefault();

            if (data == null)
                return NotFound(new { message = "找不到該行程" });
            var itineraryDetailList = data.ItineraryDetail.Split('#')
                                                          .Select(segment => segment.Trim())
                                                          .Where(segment => !string.IsNullOrEmpty(segment))
                                                          .ToList();
            var result = new Itineray_Detail
            {
                ItinerarySystemId = data.ItinerarySystemId,
                ItineraryName = data.ItineraryName,
                ActivityName = data.ActivityName,
                ActivityId = data.ActivityId,
                Price = data.Price,
                AreaName = data.AreaName,
                ItineraryBatch = data.ItineraryBatch,
                ImagePath = data.ImagePath,
                ItineraryDetails = itineraryDetailList,
                ItineraryBrief = data.ItineraryBrief,
                ItineraryNote = data.ItineraryNote
            };

            return Ok(result);
        }

        [HttpPost("search")]
        public ActionResult<IEnumerable<Itinerary_List>> Search([FromBody] SearchForm searchForm)

        {

            var query = _JP.Itineraries.AsQueryable();

            if (!string.IsNullOrEmpty(searchForm.Name))
            {
                query = query.Where(i => i.ItineraryName.Contains(searchForm.Name));
            }

            if (!string.IsNullOrEmpty(searchForm.Location))
            {
                query = query.Where(i => i.AreaSystem.AreaName.Contains(searchForm.Location));
            }

            if (!string.IsNullOrEmpty(searchForm.Month))
            {
                query = query.Where(i => i.ItineraryDates.Any(d => d.DepartureDate.Substring(5, 7) == searchForm.Month));
            }

            if (searchForm.ActivityId != 0)  // 使用 HasValue 檢查
            {
                query = query.Where(i => i.ActivitySystem.ActivitySystemId == searchForm.ActivityId);
            }

            var result = query.Select(n => new Itinerary_List
            {
                ItinerarySystemId = n.ItinerarySystemId,
                ItineraryName = n.ItineraryName ?? "",
                DepartureDate = n.ItineraryDates.Select(d => d.DepartureDate).ToList(),
                ActivityId = n.ActivitySystem.ActivitySystemId,
                AreaName = n.AreaSystem != null ? n.AreaSystem.AreaName : "",
                AvailableDate = n.Available,
                Price = n.Price,
                ImagePath = n.Images.Where(i => i.ItinerarySystemId == n.ItinerarySystemId)
                                   .Select(i => i.ImagePath)
                                   .FirstOrDefault()
            }).ToList();

            switch (searchForm.SortBy?.ToLower())
            {
                case "trendy":  // 最近期
                    result = result.OrderBy(i => i.DepartureDate.FirstOrDefault()).ToList();
                    break;
                case "latest":  // 最優惠
                    result = result.OrderBy(i => i.Price).ToList();
                    break;
                default:  // "popular" 或其他 - 最熱門
                    result = result.OrderBy(i => i.ItineraryName).ToList();  // 這裡可以改成依照您的熱門定義來排序
                    break;
            }

            return Ok(result);
        }

        [HttpGet("related/{activityId}")]
        public async Task<ActionResult<IEnumerable<Itinerary_List>>> GetRelatedItineraries(int activityId)
        {
            var relatedItineraries = await _JP.Itineraries
                .Where(i => i.ActivitySystem.ActivitySystemId == activityId)
                .Take(4)  // 多取一個，因為前端會過濾掉當前行程
                .Select(i => new Itinerary_List
                {
                    ItinerarySystemId = i.ItinerarySystemId,
                    ItineraryName = i.ItineraryName,
                    DepartureDate = i.ItineraryDates.Select(d => d.DepartureDate).ToList(),
                    ActivityId = i.ActivitySystem.ActivitySystemId,
                    AreaName = i.AreaSystem.AreaName,
                    ImagePath = i.Images.Where(i => i.ItinerarySystemId == i.ItinerarySystemId)
                                    .Select(i => i.ImagePath)
                                    .FirstOrDefault(),
                    AvailableDate = i.Available,
                    Price = i.Price
                }).ToListAsync();

            return Ok(relatedItineraries);
        }
    }
}
