using Microsoft.AspNetCore.Mvc;
using JP_FrontWebAPI.DTOs.Itinerary;
using JP_FrontWebAPI.DTOs.Shared;
using JP_FrontWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;
using JP_FrontWebAPI.DTOs.DTOs.Itinerary;

namespace JP_FrontWebAPI.Controllers
{
    [Route("api/Product")]
    [EnableCors ("All")]
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

         [HttpGet("activityNames")]
        public ActionResult<IEnumerable<Activity>> GetActivityNames()
        {
            var activityNames = _JP.Activities
                .Select(a => new Activity
                {
                    ActivitySystemId = a.ActivitySystemId,
                    ActivityName = a.ActivityName
                })
                .Distinct()
                .ToList();

            return Ok(activityNames);
        }

        [HttpGet("list")]
        public ActionResult<IEnumerable<Itinerary_List>> GetList()
        {
            var datas = _JP.Itineraries.Select(n => new Itinerary_List
            {
                ItinerarySystemId = n.ItinerarySystemId,
                ItineraryName = n.ItineraryName ?? "",  // 使用空合并运算符
                AreaName = n.AreaSystem != null ? n.AreaSystem.AreaName : "",  // 添加空检查
                ItineraryDate = n.ItineraryDates.Select(d => d.DepartureDate).ToList(), // 添加日期列表
                ActivityId = n.ActivitySystem.ActivitySystemId,  // 添加活动ID
                Stock = n.Stock ?? 0,  // 使用空合并运算符
                Price = n.Price ?? 0m,  // 使用空合并运算符
                ImagePath = n.Images.Where(i => i.ItinerarySystemId == n.ItinerarySystemId)
                                    .Select(i => i.ImagePath)
                                    .FirstOrDefault()
            }).ToList();
            return Ok(datas);
        }

        [HttpGet("{id}")]
        public ActionResult<Itineray_Detail> GetItineraryById(int id)
        {
            var data = _JP.Itineraries.Where(n => n.ItinerarySystemId == id)
                .Select(n => new Itineray_Detail
                {
                    ItinerarySystemId = n.ItinerarySystemId,
                    ItineraryName = n.ItineraryName,
                    ActivityName = n.ActivitySystem.ActivityName,
                    Stock = (int)n.Stock,
                    Price = (decimal)n.Price,
                    AreaName = n.AreaSystem.AreaName,
                    ItineraryDates = n.ItineraryDates.Select(d => d.DepartureDate).ToList(),
                    ImagePath = n.Images.Where(i => i.ItinerarySystemId == n.ItinerarySystemId)
                                        .Select(i => i.ImagePath).ToList(),
                    ItineraryDetail = n.ItineraryDetail,
                    ItineraryBrief = n.ItineraryBrief,
                    ItineraryNote = n.ItineraryNotes
                }).FirstOrDefault();

            if (data == null)
                return NotFound(new { message = "找不到該行程" });

            return Ok(data);
        }

    }
}
