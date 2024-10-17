namespace prjJapanTravel_BackendMVC.ViewModels.OrderViewModels
{
    public class TicketOrderDetailViewModel
    {
        public string 會員 { get; set; }
        public string 訂單編號 { get; set; }
        public string 出發地點 { get; set; }
        public string 抵達地點 { get; set; }
        public string 航線描述 { get; set; }
        public int 數量 { get; set; }
        public decimal 單價 { get; set; }
        public decimal 總金額 { get; set; }
        public string 優惠券名稱 { get; set; }
        public decimal? 優惠金額 { get; set; }
    }
}
