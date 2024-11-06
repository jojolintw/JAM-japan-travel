namespace JP_FrontWebAPI.DTOs.Order
{
    public class OrderDTO
    {
        public string 訂單編號 {  get; set; }
        public int 會員Id {  get; set; }
        public string 會員名稱 {  get; set; }
        public string 會員信箱 { get; set; }
        public string 會員電話 { get; set; }
        public int? 優惠券Id { get; set; }
        public string 優惠券名稱 { get; set; }
        public DateTime 下單時間 { get; set; }
        public int 訂單狀態編號 {  get; set; }
        public string 訂單狀態 { get; set; }
        public int 付款狀態編號 { get; set; }
        public string 付款狀態 { get; set; }
        public decimal 總金額 {  get; set; }
        public string 備註 { get; set; }

    }
}
