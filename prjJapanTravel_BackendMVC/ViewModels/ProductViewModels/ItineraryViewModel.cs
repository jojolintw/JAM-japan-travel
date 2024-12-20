﻿using Microsoft.AspNetCore.Mvc.Rendering;
using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.ViewModels.ProductViewModels
{
    public class ItineraryViewModel
    {
        public int 行程系統編號 { get; set; }

        public string 行程編號 { get; set; }

        public string 行程名稱 { get; set; }

        public int? 體驗項目編號 { get; set; }
        public string 體驗項目 { get; set; }

        public string 是否可報名 { get; set; }

        public decimal? 價格 { get; set; }
        public SelectList 體驗項目選項 { get; set; }
        public int? 體驗主題編號 { get; set; }
        public SelectList 體驗主題選項 { get; set; }
        public string 體驗主題 { get; set; }
        public SelectList 地區選項 { get; set; }
        public int? 地區編號 { get; set; }
        public string 地區 { get; set; }
        public List<ItineraryDate> 行程批次 { get; set; }
        public List<Image> 行程圖片 { get; set; }
        public List<ImageViewModel> imageViewModel { get; set; }
        public string 行程詳情 { get; set; }

        public string 行程簡介 { get; set; }

        public string 行程注意事項 { get; set; }

        public virtual Activity ActivitySystem { get; set; }

        public virtual Area AreaSystem { get; set; }

        public virtual ICollection<ItineraryDate> ItineraryDates { get; set; } = new List<ItineraryDate>();

        public virtual Image ItineraryPicSystem { get; set; }

        public virtual ICollection<MyCollection> MyCollections { get; set; } = new List<MyCollection>();

        public virtual Theme ThemeSystem { get; set; }
    }
}