using Microsoft.AspNetCore.Http;

namespace prjJapanTravel_BackendMVC.ViewModels.Ports
{
    public class PortImageViewModel
    {
        public int PortImageId { get; set; }             // 圖片的唯一 ID
        public int PortId { get; set; }                  // 關聯的 Port ID
        public string PortImagePath { get; set; }        // 圖片的相對路徑
        public string PortImageDescription { get; set; } // 圖片的描述

        // 用於圖片上傳的檔案
        public IFormFile ImageFile { get; set; }         // 用於接收前端上傳的圖片文件
    }
}
