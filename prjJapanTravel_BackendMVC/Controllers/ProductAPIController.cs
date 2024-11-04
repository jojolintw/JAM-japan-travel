using JP_FrontWebAPI.DTOs.DTOs.Itinerary;
using Microsoft.AspNetCore.Mvc;
using prjJapanTravel_BackendMVC.DTOs.Itinerary;
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
        public ActionResult<IEnumerable<Itinerary_List>> GetList()
        {
            var datas = _JP.Itineraries.Select(n => new Itinerary_List
            {
                ItinerarySystemId = n.ItinerarySystemId,
                ItineraryName= n.ItineraryName,
                // Stock = (int)n.Stock,
                // Price = (decimal)n.Price,
                // ImageName = n.Images.FirstOrDefault().ImageName
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

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] ItineraryViewModel itiModel)
        {
            if (id != itiModel.行程系統編號)
                return BadRequest(new { message = "ID不匹配" });

            var itinerary = _JP.Itineraries.FirstOrDefault(n => n.ItinerarySystemId == id);
            if (itinerary == null)
                return NotFound(new { message = "找不到该行程" });

            try
            {
                // 更新基本信息
                itinerary.ItineraryId = itiModel.行程編號;
                itinerary.ItineraryName = itiModel.行程名稱;
                itinerary.ActivitySystemId = itiModel.體驗項目編號;
                itinerary.Stock = itiModel.總團位;
                itinerary.Price = itiModel.價格;
                itinerary.AreaSystemId = itiModel.地區編號;
                itinerary.ItineraryDetail = itiModel.行程詳情;
                itinerary.ItineraryBrief = itiModel.行程簡介;
                itinerary.ItineraryNotes = itiModel.行程注意事項;

                // 更新行程日期
                var existingDates = _JP.ItineraryDates.Where(d => d.ItinerarySystemId == id).ToList();
                _JP.ItineraryDates.RemoveRange(existingDates);

                if (itiModel.行程日期 != null)
                {
                    foreach (var date in itiModel.行程日期)
                    {
                        _JP.ItineraryDates.Add(new ItineraryDate
                        {
                            ItinerarySystemId = id,
                            DepartureDate = date.DepartureDate
                        });
                    }
                }

                _JP.SaveChanges();
                return Ok(new { message = "更新成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
