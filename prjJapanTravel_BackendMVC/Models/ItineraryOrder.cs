﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjJapanTravel_BackendMVC.Models;

public partial class ItineraryOrder
{
    public int ItineraryOrderId { get; set; }

    public string ItineraryOrderNumber { get; set; }

    public int MemberId { get; set; }

    public int ItineraryDateSystemId { get; set; }

    public int Quantity { get; set; }

    public DateTime OrderTime { get; set; }

    public int PaymentMethodId { get; set; }

    public int PaymentStatusId { get; set; }

    public DateTime? PaymentTime { get; set; }

    public int OrderStatusId { get; set; }

    public int? CouponId { get; set; }

    public decimal TotalAmount { get; set; }

    public string Remarks { get; set; }

    public int? ReviewRating { get; set; }

    public string ReveiwContent { get; set; }

    public DateTime? ReviewTime { get; set; }

    public bool? ReviewStatus { get; set; }

    public string RepresentativeLastName { get; set; }

    public string RepresentativeFirstName { get; set; }

    public string RepresentativeIdnumber { get; set; }

    public string RepresentativePassportNumber { get; set; }

    public string RepresentativePhoneNumber { get; set; }
}