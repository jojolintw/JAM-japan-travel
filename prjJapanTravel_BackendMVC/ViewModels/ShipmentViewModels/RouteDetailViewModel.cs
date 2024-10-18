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
        public List<byte[]> Images { get; set; }
        public int RouteImageId { get; set; }
        public byte[] Image { get; set; } // 或使用 IFormFile 來上傳文件
        public IFormFile ImageFile { get; set; } // 使用 IFormFile 來接收檔案
        public string ImageDescription { get; set; }

        public List<string> ImageDescriptions { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}
