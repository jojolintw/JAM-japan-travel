﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjJapanTravel_BackendMVC.Models;

public partial class Activity
{
    public int ActivitySystemId { get; set; }

    public string ActivityName { get; set; }

    public string ActivityDetail { get; set; }

    public virtual ICollection<Itinerary> Itineraries { get; set; } = new List<Itinerary>();
}