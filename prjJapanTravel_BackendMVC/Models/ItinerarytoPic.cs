﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjJapanTravel_BackendMVC.Models;

public partial class ItinerarytoPic
{
    public int? ItineraryPicSystemId { get; set; }

    public int? ItinerarySystemId { get; set; }

    public virtual Image ItineraryPicSystem { get; set; }

    public virtual Itinerary ItinerarySystem { get; set; }
}