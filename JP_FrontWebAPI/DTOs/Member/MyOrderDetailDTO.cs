namespace JP_FrontWebAPI.DTOs.Member
{
    public class MyOrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int ItineraryDateSystemId { get; set; }

        public int ItinerarySystemId { get; set; }

        public string ItineraryId {  get; set; }

        public string ItineraryName { get; set; }
        public string DepartureDate { get; set; }
        public int Quantity { get; set; }
        public string TotalPrice { get; set; }

    }
}
