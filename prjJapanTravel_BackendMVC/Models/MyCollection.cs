﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjJapanTravel_BackendMVC.Models;

public partial class MyCollection
{
    public int MyCollectionId { get; set; }

    public int MemberId { get; set; }

    public int ItinerarySystemId { get; set; }

    public virtual Itinerary ItinerarySystem { get; set; }

    public virtual Member Member { get; set; }
}