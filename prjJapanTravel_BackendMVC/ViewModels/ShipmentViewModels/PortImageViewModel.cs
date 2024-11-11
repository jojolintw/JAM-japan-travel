using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.ViewModels.Ports
{
    public class PortDetailsViewModel
    {
        public int PortId { get; set; }
        public string PortName { get; set; }
        public string City { get; set; }
        public string CityDescription1 { get; set; }
        public string CityDescription2 { get; set; }
        public string PortGoogleMap { get; set; }
        //public List<PortImageViewModel> PortImages { get; set; } = new List<PortImageViewModel>();
        public List<PortImage> PortImages { get; set; }

    }

    public class PortImageViewModel
    {
        public int PortImageId { get; set; }
        public int PortId { get; set; }

        // 用於顯示圖片的 Base64 字串
        public string PortImageBase64 { get; set; }

        // 圖片描述
        public string PortImageDescription { get; set; }

        // 用於圖片上傳的檔案
        public IFormFile ImageFile { get; set; }
    }
}
