namespace JP_FrontWebAPI.DTOs.Order
{
    public class CheckOutDTO
    {
        public int price { get; set; }
        public int? couponId { get; set; }
        public string address { get; set; }
        public int amount { get; set; }
    }
}
