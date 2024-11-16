using JP_FrontWebAPI.Models;
using System.Text.Json.Serialization;

namespace JP_FrontWebAPI.DTOs.Itinerary
{
    public class Itinerary_List
    {
        public int ItinerarySystemId { get; set; }
        public string? ItineraryName { get; set; }
        public List<string?> DepartureDate { get; set; }
        public List<Itinerary_Date> ItineraryBatch { get; set; }
        public Theme_Activity Theme_Activity { get; set; }
        public int? ActivitySystemId { get; set; }
        public string? AreaName { get; set; }
        public string? ImagePath { get; set; }
        public string? AvailableDate { get; set; }
        public decimal? Price { get; set; }
        public double? StarRate {  get; set; }
    }
}
