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

    public virtual ICollection<ArticleHashtag> ArticleHashtags { get; set; } = new List<ArticleHashtag>();

    public virtual ICollection<ArticlePic> ArticlePics { get; set; } = new List<ArticlePic>();

    public virtual ArticleStatus ArticleStatusnumberNavigation { get; set; }

    public virtual Member Member { get; set; }
}