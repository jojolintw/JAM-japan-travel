﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjJapanTravel_BackendMVC.Models;

public partial class City
{
    public int CityId { get; set; }

    public string City1 { get; set; }

    public string CityCode { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}