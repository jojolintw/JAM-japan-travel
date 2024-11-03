using Microsoft.AspNetCore.Mvc;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.ProductViewModels;

namespace prjJapanTravel_BackendMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private JapanTravelContext _JP;
        IWebHostEnvironment _enviroment;
        
        public ProductAPIController(JapanTravelContext context, IWebHostEnvironment environment)
        {
            _JP = context;
            _enviroment = environment;
        }

        [HttpGet("list")]
        public ActionResult<IEnumerable<ItineraryViewModel>> List()
        {
            var datas = _JP.Itineraries.Select(n => new ItineraryViewModel()
            {
                行程系統編號 = n.ItinerarySystemId,
                行程編號 = n.ItineraryId,
                行程名稱 = n.ItineraryName,
                體驗主題 = n.ActivitySystem.ThemeSystem.ThemeName,
                體驗項目 = n.ActivitySystem.ActivityName,
                總團位 = n.Stock,
                價格 = n.Price,
                地區 = n.AreaSystem.AreaName,
                行程日期 = n.ItineraryDates.Where(date => date.ItinerarySystemId == n.ItinerarySystemId).ToList(),
                行程圖片 = n.Images.Where(img => img.ItinerarySystemId == n.ItinerarySystemId).ToList(),
                行程詳情 = n.ItineraryDetail,
                行程簡介 = n.ItineraryBrief,
                行程注意事項 = n.ItineraryNotes
            });
            return Ok(datas);
        }

        [HttpGet("create-options")]
        public ActionResult GetCreateOptions()
        {
            var options = new CreateListViewModel
            {
                areaList = _JP.Areas.ToList(),
                themeList = _JP.Themes.ToList(),
                activityList = _JP.Activities.ToList()
            };
            return Ok(options);
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] ItineraryViewModel itimodel)
        {
            try 
            {
                Itinerary itinerary = new Itinerary
                {
                    ItineraryId = itimodel.行程編號,
                    ItineraryName = itimodel.行程名稱,
                    ActivitySystemId = itimodel.體驗項目編號,
                    Stock = itimodel.總團位,
                    Price = itimodel.價格,
                    AreaSystemId = itimodel.地區編號,
                    ItineraryDetail = itimodel.行程詳情,
                    ItineraryBrief = itimodel.行程簡介,
                    ItineraryNotes = itimodel.行程注意事項
                };

                _JP.Itineraries.Add(itinerary);
                _JP.SaveChanges();

                // 行程日期
                if (itimodel.行程日期 != null && itimodel.行程日期.Count > 0)
                {
                    foreach (var date in itimodel.行程日期)
                    {
                        _JP.ItineraryDates.Add(new ItineraryDate
                        {
                            ItinerarySystemId = itinerary.ItinerarySystemId,
                            DepartureDate = date.DepartureDate
                        });
                    }
                    _JP.SaveChanges();
                }

                return Ok(new { message = "新增成功", itineraryId = itinerary.ItinerarySystemId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var iti = _JP.Itineraries.FirstOrDefault(n => n.ItinerarySystemId == id);
            if (iti == null)
                return NotFound(new { message = "找不到該行程" });

            try
            {
                var itineraryTimes = _JP.ItineraryDates.Where(n => n.ItinerarySystemId == id).ToList();
                var itineraryImages = _JP.Images.Where(n => n.ItinerarySystemId == id).ToList();

                _JP.ItineraryDates.RemoveRange(itineraryTimes);
                _JP.Images.RemoveRange(itineraryImages);
                _JP.Itineraries.Remove(iti);

                _JP.SaveChanges();

                return Ok(new { message = "刪除除成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
