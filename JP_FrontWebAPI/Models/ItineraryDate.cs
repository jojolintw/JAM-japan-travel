﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JP_FrontWebAPI.Models;

public partial class ItineraryDate
{
    public int ItineraryDateSystemId { get; set; }

    public int ItinerarySystemId { get; set; }

    public string DepartureDate { get; set; }

    public int? Stock { get; set; }

    public virtual ICollection<ItineraryOrderItem> ItineraryOrderItems { get; set; } = new List<ItineraryOrderItem>();

    public virtual Itinerary ItinerarySystem { get; set; }
}