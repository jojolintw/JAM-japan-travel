using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.ViewModels.ShipmentViewModels
{
    public class RouteDetailViewModel
    {
        public int RouteId { get; set; }
        public string OriginPortName { get; set; }
        public string DestinationPortName { get; set; }
        public decimal Price { get; set; }
        public string RouteDescription { get; set; }
        public byte[] Image { get; set; } // 這應該是 byte[]，對應到資料庫中的 Image
        public string ImageDescription { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}
