namespace JP_FrontWebAPI.DTOs.Member
{
    public class MyOrderDTO
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public int PaymentMehtodId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderTime { get; set; }
    }
}
