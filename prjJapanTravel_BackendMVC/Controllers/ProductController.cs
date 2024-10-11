using Microsoft.AspNetCore.Mvc;
using prjJapanTravel_BackendMVC.Models;

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
            
            var datas = from iti in _JP.Itineraries
                        select iti;
            return View(datas);
        }

        public IActionResult ItineraryCreate()
        {
            return View();
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
            Itinerary iti = _JP.Itineraries.FirstOrDefault(n => n.ItinerarySystemId == id);
            if (iti == null)
                return RedirectToAction("List");
            return View(iti);
        }
    }
}
