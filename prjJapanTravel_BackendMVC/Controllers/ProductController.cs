using Microsoft.AspNetCore.Mvc;
using prjJapanTravel_BackendMVC.Models;
using prjJapanTravel_BackendMVC.ViewModels.ProductViewModels;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class ProductController : Controller
    {
        private JapanTravelContext _JP;
        public ProductController(JapanTravelContext JP)
        {
            _JP = JP;
        }
        public IActionResult List()
        {
            var datas = _JP.Itineraries.Select(n => new ItineraryListViewModel()
            {
                行程系統編號 = n.ItinerarySystemId,
                行程編號 = n.ItineraryId,
                行程名稱 = n.ItineraryName,
                體驗項目 = n.ActivitySystem.ActivityName,
                總團位 = n.Stock,
                價格 = n.Price,
                體驗主題 = n.ThemeSystem.ThemeName,
                地區 = n.AreaSystem.AreaName,
                行程圖片 = n.ItineraryPicSystemId,
                行程詳情 = n.ItineraryDetail,
                行程簡介 = n.ItineraryBrief,
                行程注意事項 = n.ItineraryNotes
            });
            return View(datas);
        }

        public IActionResult ItineraryCreate()
        {
            CreateListViewModel c = new CreateListViewModel();
            c.areaList = _JP.Areas.ToList();
            c.activityList = _JP.Activities.ToList();
            return View(c);
        }

        [HttpPost]
        public IActionResult ItineraryCreate(Itinerary iti)
        {
            _JP.Itineraries.Add(iti);
            return RedirectToAction("List");
        }

        public IActionResult ItineraryDelete(int? id)
        {
            Itinerary iti = _JP.Itineraries.FirstOrDefault(n => n.ItinerarySystemId == id);
            if (iti != null)
            {
                var itineraryTimes = _JP.ItineraryDates.Where(n => n.ItinerarySystemId == id).ToList();
                var itineraryImages = _JP.Images.Where(n => n.ItinerarySystemId == id).ToList();

                _JP.ItineraryDates.RemoveRange(itineraryTimes);
                _JP.Images.RemoveRange(itineraryImages);
                _JP.Itineraries.Remove(iti);

                _JP.SaveChanges();
            }
            return RedirectToAction("List");
        }
        public IActionResult ItineraryEdit(int? id)
        {
            var data = _JP.Itineraries.Where(n => n.ItinerarySystemId == id).Select(n => new ItineraryListViewModel()
            {
                行程系統編號 = n.ItinerarySystemId,
                行程編號 = n.ItineraryId,
                行程名稱 = n.ItineraryName,
                體驗項目編號 = n.ActivitySystemId,
                體驗項目 = n.ActivitySystem.ActivityName,
                總團位 = n.Stock,
                價格 = n.Price,
                體驗主題編號 = n.ThemeSystemId,
                體驗主題 = n.ThemeSystem.ThemeName,
                地區編號 = n.AreaSystemId,
                地區 = n.AreaSystem.AreaName,
                行程圖片 = n.ItineraryPicSystemId,
                行程詳情 = n.ItineraryDetail,
                行程簡介 = n.ItineraryBrief,
                行程注意事項 = n.ItineraryNotes
            }).FirstOrDefault();
            return View(data);
        }
        [HttpPost]
        public IActionResult ItineraryEdit(ItineraryListViewModel itiModel)
        {
            var itinerary = _JP.Itineraries.FirstOrDefault(n => n.ItinerarySystemId == itiModel.行程系統編號);
            if (itinerary == null)
            {
                return RedirectToAction("List");
            }

            itinerary.ItineraryId = itiModel.行程編號;
            itinerary.ItineraryName = itiModel.行程名稱;
            itinerary.ActivitySystemId = itiModel.體驗項目編號;
            itinerary.Stock = itiModel.總團位;
            itinerary.Price = itiModel.價格;
            itinerary.ThemeSystemId = itiModel.體驗主題編號;
            itinerary.AreaSystemId = itiModel.地區編號;
            itinerary.ItineraryPicSystemId = itiModel.行程圖片;
            itinerary.ItineraryDetail = itiModel.行程詳情;
            itinerary.ItineraryBrief = itiModel.行程簡介;
            itinerary.ItineraryNotes = itiModel.行程注意事項;

            _JP.SaveChanges();
            return RedirectToAction("List");
            
        }
    }
}
