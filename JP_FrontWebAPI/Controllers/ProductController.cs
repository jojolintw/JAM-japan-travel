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
        [HttpGet("theme_activities")]
        public ActionResult<List<Theme_Activity>> GetAllThemeActivities()
        {
            try
            {
                var themeActivities = _JP.Themes
                    .Select(t => new Theme_Activity
                    {
                        ThemeSystemId = t.ThemeSystemId,
                        ThemeName = t.ThemeName,
                        Activities = _JP.Activities
                            .Where(a => a.ThemeSystemId == t.ThemeSystemId)
                            .Select(a => new Activity
                            {
                                ActivitySystemId = a.ActivitySystemId,
                                ActivityName = a.ActivityName
                            })
                            .ToList()
                    })
                    .ToList();

                if (!themeActivities.Any())
                {
                    return NotFound("未找到相關主題");
                }

                return Ok(themeActivities);
            }
            catch (Exception ex)
            {
                return BadRequest($"獲取主题失敗: {ex.Message}");
            }
        }

        // 根據主題ID獲取行程列表
        [HttpGet("itineraries/theme/{themeId}")]
        public ActionResult<List<Itinerary_List>> GetItinerariesByTheme(int themeId)
        {
            try
            {
                var itineraries = _JP.Itineraries
                    .Where(i => i.ActivitySystem.ThemeSystemId == themeId)
                    .Select(i => new Itinerary_List
                    {
                        // 映射所需屬性
                        ItinerarySystemId = i.ItinerarySystemId,
                        ItineraryName = i.ItineraryName,
                        // ... 其他屬性
                    })
                    .ToList();

                return Ok(itineraries);
            }
            catch (Exception ex)
            {
                return BadRequest($"獲取行程失敗: {ex.Message}");
            }
        }

        // 根據活動ID獲取行程列表
        [HttpGet("itineraries/activity/{activityId}")]
        public ActionResult<List<Itinerary_List>> GetItinerariesByActivity(int activityId)
        {
            try
            {
                var itineraries = _JP.Itineraries
                    .Where(i => i.ActivitySystemId == activityId)
                    .Select(i => new Itinerary_List
                    {
                        // 映射所需屬性
                        ItinerarySystemId = i.ItinerarySystemId,
                        ItineraryName = i.ItineraryName,
                        // ... 其他屬性
                    })
                    .ToList();

                return Ok(itineraries);
            }
            catch (Exception ex)
            {
                return BadRequest($"獲取行程失敗: {ex.Message}");
            }
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
                ActivitySystemId = n.ActivitySystem.ActivitySystemId,
                AvailableDate = n.Available,
                Price = n.Price,
                ImagePath = n.Images.Where(img => img.ItinerarySystemId == n.ItinerarySystemId && img.ImageName == "封面")
                                    .Select(i => "https://localhost:7100/images/Product/" + i.ImagePath)
                                    .FirstOrDefault(),
            }).ToList();

            var starRates = _JP.StarRatings
                            .Select(sr => new { sr.ItinerarySystemId, sr.StarRate })
                            .ToDictionary(sr => sr.ItinerarySystemId, sr => sr.StarRate);

            var result = datas.Select(n => new Itinerary_List
            {
                ItinerarySystemId = n.ItinerarySystemId,
                ItineraryName = n.ItineraryName,
                AreaName = n.AreaName,
                DepartureDate = n.DepartureDate.ToList(),
                ActivitySystemId = n.ActivitySystemId,
                AvailableDate = n.AvailableDate,
                Price = n.Price,
                ImagePath = n.ImagePath,
                StarRate = starRates.TryGetValue(n.ItinerarySystemId, out var starRate) ? starRate : 0
            }).ToList();

            return Ok(result);
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
                    ActivitySystemId = n.ActivitySystem.ActivitySystemId,
                    Price = (decimal)n.Price,
                    AreaName = n.AreaSystem.AreaName,
                    ItineraryBatch = n.ItineraryDates.Where(batch => batch.ItinerarySystemId == n.ItinerarySystemId)
                                                     .Select(d => new Itinerary_Date
                                                     {
                                                         ItineraryDateSystemId = d.ItineraryDateSystemId,
                                                         DepartureDate = d.DepartureDate,
                                                         Stock = d.Stock
                                                     }).ToList(),
                    ImagePath = n.Images.Where(i => i.ItinerarySystemId == n.ItinerarySystemId && i.ImageName == "內文")
                                        .Select(i => "https://localhost:7100/images/Product/" + i.ImagePath).ToList(),
                    Theme_Activity = new Theme_Activity // 直接在这里填充主题活动信息
                    {
                        ThemeSystemId = (int)n.ActivitySystem.ThemeSystemId, // 确保 ActivitySystem 有 ThemeSystemId 属性
                        ThemeName = n.ActivitySystem.ThemeSystem.ThemeName, // 确保 ActivitySystem 有 ThemeSystem 属性
                        Activities = new List<Activity>
                        {
                            new Activity
                            {
                                ActivitySystemId = (int)n.ActivitySystem.ActivitySystemId,
                                ActivityName = n.ActivitySystem.ActivityName
                            }
                        }
                    },
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
                ActivitySystemId = data.ActivitySystemId,
                Price = data.Price,
                AreaName = data.AreaName,
                ItineraryBatch = data.ItineraryBatch,
                ImagePath = data.ImagePath,
                ItineraryDetails = itineraryDetailList,
                ItineraryBrief = data.ItineraryBrief,
                ItineraryNote = data.ItineraryNote,
                Theme_Activity = data.Theme_Activity
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
                ActivitySystemId = n.ActivitySystem.ActivitySystemId,
                AreaName = n.AreaSystem != null ? n.AreaSystem.AreaName : "",
                AvailableDate = n.Available,
                Price = n.Price,
                ImagePath = n.Images.Where(i => i.ItinerarySystemId == n.ItinerarySystemId && i.ImageName == "封面")
                                   .Select(i => "https://localhost:7100/images/Product/" + i.ImagePath)
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
                    ActivitySystemId = i.ActivitySystem.ActivitySystemId,
                    AreaName = i.AreaSystem.AreaName,
                    ImagePath = i.Images.Where(i => i.ItinerarySystemId == i.ItinerarySystemId && i.ImageName == "封面")
                                    .Select(i => "https://localhost:7100/images/Product/" + i.ImagePath)
                                    .FirstOrDefault(),
                    AvailableDate = i.Available,
                    Price = i.Price
                }).ToListAsync();

            return Ok(relatedItineraries);
        }

        [HttpGet("detail/comment/{itinerarySystemId}")]
        public ActionResult<OrderComments> GetCommentsByItineraryId(int itinerarySystemId)
        {
            var orderCommentsData = _JP.Orders
                .Where(o => o.ItineraryOrderItems.Any(io => io.ItineraryDateSystem.ItinerarySystemId == itinerarySystemId))
                .Select(o => new OrderComments
                {
                    MemberId = (int)o.MemberId,
                    MemberName = o.Member.MemberName,
                    OrderId = o.OrderId,
                    Comments = o.ItineraryOrderItems
                        .Where(io => io.ItineraryDateSystem.ItinerarySystemId == itinerarySystemId) // 只选择与指定行程相关的评论
                        .Select(io => new Comments
                        {
                            ItinerarySystemId = io.ItineraryDateSystem.ItinerarySystemId,
                            CommentStar = io.CommentStar,
                            CommentContent = io.CommentContent,
                            CommentDate = io.CommentTime.ToString() // 确保 CommentTime 处理为字符串
                        }).ToList()
                });

            if (orderCommentsData == null)
            {
                return NotFound();
            }

            return Ok(orderCommentsData);
        }
    }
}
