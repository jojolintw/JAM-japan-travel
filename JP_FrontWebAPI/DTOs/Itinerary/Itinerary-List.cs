using JP_FrontWebAPI.Models;

namespace JP_FrontWebAPI.DTOs.Itinerary
{
    public class Itinerary_List
    {
        public int ItinerarySystemId { get; set; }
        public string ItineraryName { get; set; }
        public List<string> DepartureDate { get; set; }
        public List<ItineraryDate> ItineraryBatch { get; set; }
        public int ActivityId { get; set; }
        public string? AreaName { get; set; }
        public string? ImagePath { get; set; }
        public string AvailableDate { get; set; }
        public decimal? Price { get; set; }
    }
}
