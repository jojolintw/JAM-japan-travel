namespace JP_FrontWebAPI.DTOs.Member
{
    public class MyCouponDTO
    {
        public int CouponId { get; set; }

        public string CouponName { get; set; }

        public decimal Discount { get; set; }

        public string ExpirationDate { get; set; }
        public int MemberLevelId { get; set; }
    }
}
