using Microsoft.AspNetCore.Mvc;
using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class ProductController : Controller
    {
        private Itinerary _ITI;
        public ProductController(Itinerary ITI)
        {
            _ITI = ITI;
        }

        public IActionResult Index()
        {
            JapanTravelContext jp = new JapanTravelContext();
            var datas = from iti in jp.Itineraries
                        select iti;
            return View();
        }

        public IActionResult ItineraryCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ItineraryCreate(Itinerary iti)
        {
            var ItineraryMain = new Itinerary(); 
            //ItineraryMain.
            return RedirectToAction("List");
        }

        public IActionResult ItineraryDelete(int? id)
        {
            //Itinerary iti = jp.Itinerary行程s.FirstOrDefault(n => n.行程系統編號 == id);
            //if (/*iti != null*/)
            //{
            //    var itineraryTimes = jp.ItineraryTime行程批次s.Where(n => n.行程系統編號 == id).ToList();
            //    var itineraryPics = jp.Picture圖片s.Where(n => n.行程系統編號 == id).ToList();
            //    jp.ItineraryTime行程批次s.RemoveRange(itineraryTimes);
            //    jp.Picture圖片s.RemoveRange(itineraryPics);
            //    jp.Itinerary行程s.Remove(iti);

            //    jp.SaveChanges();
            //}
            return RedirectToAction("List");
        }

        public IActionResult ItineraryEdit(int? id)
        {
            //Itinerary iti = jp.Itinerary行程s.FirstOrDefault(n => n.行程系統編號 == id);
            //if (iti == null)
            //    return RedirectToAction("List");
            return View(/*iti*/);
        }
    }
}
