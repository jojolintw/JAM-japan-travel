using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.ViewModels.ShipmentViewModels
{
    public class RouteDetailViewModel
    {
        public int RouteId { get; set; }
        public string OriginPortName { get; set; }
        public string DestinationPortName { get; set; }
        public string RouteDescription { get; set; }
        public decimal Price { get; set; }
        public List<Schedule> Schedules { get; set; }
        public List<RouteImage> RouteImages { get; set; }
    }

}
