using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.ViewModels.OrderViewModels
{
    public class TicketOrderListViewModel
    {
        public int 船票訂單編號 { get; set; }

        public string 訂單編號 { get; set; }

        public int 會員編號 { get; set; }

        public DateTime 下單時間 { get; set; }

        public int 付款方式編號 { get; set; }

        public int 付款狀態編號 { get; set; }

        public DateTime? 付款時間 { get; set; }

        public int 訂單狀態編號 { get; set; }

        public int? 優惠券編號 { get; set; }

        public decimal 總金額 { get; set; }

        public string 備註 { get; set; }

        public int? 評論星級 { get; set; }

        public string 評論 { get; set; }

        public DateTime? 評論時間 { get; set; }

        public bool? 評論狀態 { get; set; }

        public string 代表人姓氏 { get; set; }

        public string 代表人名字 { get; set; }

        public string 代表人身份証字號 { get; set; }

        public string 代表人護照號碼 { get; set; }

        public string 代表人手機號碼 { get; set; }
        public string 會員 { get; set; }
        public string 付款方式 { get; set; }
        public string 付款狀態 { get; set; }
        public string 訂單狀態 { get; set; }
        public string 優惠券 { get; set; }


        public virtual Coupon Coupon { get; set; }

        public virtual Member Member { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual PaymentStatus PaymentStatus { get; set; }

        public virtual ICollection<TicketOrderItem> TicketOrderItems { get; set; } = new List<TicketOrderItem>();
    }
}
