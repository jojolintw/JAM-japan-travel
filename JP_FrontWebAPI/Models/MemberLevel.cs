﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JP_FrontWebAPI.Models;

public partial class MemberLevel
{
    public int MemberLevelId { get; set; }

    public string MemberLevelName { get; set; }

    public decimal? Condtition { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}