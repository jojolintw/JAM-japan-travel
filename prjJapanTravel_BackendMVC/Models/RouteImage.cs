﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjJapanTravel_BackendMVC.Models;

public partial class RouteImage
{
    public int RouteImageId { get; set; }

    public int RouteId { get; set; }

    public byte[] Image { get; set; }

    public string Description { get; set; }

    public virtual Route Route { get; set; }
}