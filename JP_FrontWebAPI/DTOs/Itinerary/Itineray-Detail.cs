﻿using JP_FrontWebAPI.Models;
using System.Text.Json.Serialization;

namespace JP_FrontWebAPI.DTOs.Itinerary
{
    public class Itineray_Detail
    {
        public int ItinerarySystemId { get; set; }
        public string ItineraryName { get; set; }
        public string AreaName { get; set; }
        public string ThemeName { get; set; }
        public int? ThemeSystemId { get; set; }
        public string ActivityName { get; set; }
        public int? ActivitySystemId { get; set; }
        public Theme_Activity Theme_Activity { get; set; }
        public List<string> ImagePath { get; set; }
        public List<Itinerary_Date> ItineraryBatch { get; set; }
        public decimal Price { get; set; }
        public string ItineraryDetail { get; set; }
        public List<string?> ItineraryDetails { get; set; }
        public string? ItineraryBrief { get; set; }
        public string? ItineraryNote { get; set; }
    }
}
