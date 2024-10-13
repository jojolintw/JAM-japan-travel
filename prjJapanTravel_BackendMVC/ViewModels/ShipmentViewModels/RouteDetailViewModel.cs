using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.ViewModels.ShipmentViewModels
{
    public class RouteDetailViewModel
    {
        public int RouteId { get; set; }

        public int OriginPortId { get; set; }

        public int DestinationPortId { get; set; }

        public decimal Price { get; set; }

        public string RouteDescription { get; set; }

        public virtual Port DestinationPort { get; set; }

        public virtual Port OriginPort { get; set; }
        public int ScheduleId { get; set; }
        public DateTime DepartureTime { get; set; }

        public byte[] Image { get; set; }
    }
}
