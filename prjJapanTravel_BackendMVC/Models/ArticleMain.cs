﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjJapanTravel_BackendMVC.Models;

public partial class ArticleMain
{
    public int ArticleNumber { get; set; }

    public int MemberId { get; set; }

    public DateOnly ArticleLaunchtime { get; set; }

    public int ArticleStatusnumber { get; set; }

    public string ArticleTitle { get; set; }

    public DateOnly ArticleUpdatetime { get; set; }
}