using System.Data;

namespace prjJapanTravel_BackendMVC.ViewModels.OrderViewModels
{
    public class OrderListViewModel
    {
        public int? 訂單Id { get; set; }
        public string 訂單編號 {  get; set; }
        public int? 會員編號 { get; set; }
        public int? 商品編號 { get; set; }
        public int? 數量 { get; set; }

        public DateTime? 下單時間 { get; set; }

        public int? 付款方式編號 { get; set; }

        public int? 付款狀態編號 { get; set; }
        public int? 訂單狀態編號 { get; set; }

        public int? 優惠券編號 { get; set; }

        public decimal? 總金額 { get; set; }

        public string 備註 { get; set; }

        public int? 評論星級 { get; set; }

        public string 評論 { get; set; }

        public DateTime? 評論時間 { get; set; }

        public bool? 評論狀態 { get; set; }
        public string? 優惠券 { get; set; }
        public string 商品名稱 { get; set; }
        public int 商品名稱編號 { get; set; }
        public string 會員 { get; set; }
        public string 付款方式 { get; set; }
        public DateTime? 付款時間 { get; set; }
        public string 訂單狀態 { get; set; }

    }
}
