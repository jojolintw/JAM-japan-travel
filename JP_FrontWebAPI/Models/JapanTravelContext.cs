﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JP_FrontWebAPI.Models;

public partial class JapanTravelContext : DbContext
{
    public JapanTravelContext(DbContextOptions<JapanTravelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<ArticleHashtag> ArticleHashtags { get; set; }

    public virtual DbSet<ArticleMain> ArticleMains { get; set; }

    public virtual DbSet<ArticlePic> ArticlePics { get; set; }

    public virtual DbSet<ArticleStatus> ArticleStatuses { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CityArea> CityAreas { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<HashtagMain> HashtagMains { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Itinerary> Itineraries { get; set; }

    public virtual DbSet<ItineraryDate> ItineraryDates { get; set; }

    public virtual DbSet<ItineraryOrder> ItineraryOrders { get; set; }

    public virtual DbSet<ItineraryOrderItem> ItineraryOrderItems { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemberCouponList> MemberCouponLists { get; set; }

    public virtual DbSet<MemberLevel> MemberLevels { get; set; }

    public virtual DbSet<MemberStatus> MemberStatuses { get; set; }

    public virtual DbSet<MyCollection> MyCollections { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }

    public virtual DbSet<Port> Ports { get; set; }

    public virtual DbSet<PortImage> PortImages { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<RouteImage> RouteImages { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    public virtual DbSet<TicketOrder> TicketOrders { get; set; }

    public virtual DbSet<TicketOrderItem> TicketOrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.ActivitySystemId).HasName("PK_Spot景點");

            entity.ToTable("Activity");

            entity.Property(e => e.ActivityName).HasMaxLength(50);

            entity.HasOne(d => d.ThemeSystem).WithMany(p => p.Activities)
                .HasForeignKey(d => d.ThemeSystemId)
                .HasConstraintName("FK_Activity_Theme");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK_Admin管理員");

            entity.ToTable("Admin");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Account)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.AdminName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.ImagePath)
                .HasMaxLength(50)
                .HasColumnName("imagePath");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaSystemId).HasName("PK_Area地區");

            entity.ToTable("Area");

            entity.Property(e => e.AreaName).HasMaxLength(20);
        });

        modelBuilder.Entity<ArticleHashtag>(entity =>
        {
            entity.HasKey(e => e.ArticleHashtagnumber).HasName("PK_文章hashtag中介表_1");

            entity.ToTable("ArticleHashtag");

            entity.HasOne(d => d.ArticleNumberNavigation).WithMany(p => p.ArticleHashtags)
                .HasForeignKey(d => d.ArticleNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleHashtag_ArticleMain");

            entity.HasOne(d => d.HashtagNumberNavigation).WithMany(p => p.ArticleHashtags)
                .HasForeignKey(d => d.HashtagNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleHashtag_HashtagMain");
        });

        modelBuilder.Entity<ArticleMain>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK_Article文章_1");

            entity.ToTable("ArticleMain");

            entity.Property(e => e.ArticleContent).IsRequired();
            entity.Property(e => e.ArticleLastupatetime).HasColumnType("datetime");
            entity.Property(e => e.ArticleLaunchtime).HasColumnType("datetime");
            entity.Property(e => e.ArticleTitle)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.MemberId).HasColumnName("MemberID");

            entity.HasOne(d => d.ArticleStatusnumberNavigation).WithMany(p => p.ArticleMains)
                .HasForeignKey(d => d.ArticleStatusnumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleMain_ArticleStatus");
        });

        modelBuilder.Entity<ArticlePic>(entity =>
        {
            entity.HasKey(e => e.PicNumber).HasName("PK_Article圖片_1");

            entity.ToTable("ArticlePic");

            entity.Property(e => e.PicNumber).ValueGeneratedNever();
            entity.Property(e => e.ArticleNumber).ValueGeneratedOnAdd();
            entity.Property(e => e.PicDescription)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.PicPath).HasMaxLength(50);

            entity.HasOne(d => d.ArticleNumberNavigation).WithMany(p => p.ArticlePics)
                .HasForeignKey(d => d.ArticleNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticlePic_ArticleMain");
        });

        modelBuilder.Entity<ArticleStatus>(entity =>
        {
            entity.HasKey(e => e.StatusNumber).HasName("PK_Article狀態_1");

            entity.ToTable("ArticleStatus");

            entity.Property(e => e.StatusName)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("City");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CityName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.CityArea).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CityAreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_CityArea");
        });

        modelBuilder.Entity<CityArea>(entity =>
        {
            entity.ToTable("CityArea");

            entity.Property(e => e.CityAreaName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("PK_Coupon優惠券");

            entity.ToTable("Coupon");

            entity.Property(e => e.CouponId).HasColumnName("CouponID");
            entity.Property(e => e.CouponName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ExpirationDate).HasColumnName("Expiration date");
        });

        modelBuilder.Entity<HashtagMain>(entity =>
        {
            entity.HasKey(e => e.HashtagNumber).HasName("PK_hashtag_1");

            entity.ToTable("HashtagMain");

            entity.Property(e => e.HashtagName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ItineraryPicSystemId);

            entity.ToTable("Image");

            entity.Property(e => e.ImageName).HasMaxLength(50);

            entity.HasOne(d => d.ItinerarySystem).WithMany(p => p.Images)
                .HasForeignKey(d => d.ItinerarySystemId)
                .HasConstraintName("FK_Image_Itinerary");
        });

        modelBuilder.Entity<Itinerary>(entity =>
        {
            entity.HasKey(e => e.ItinerarySystemId).HasName("PK_Itinerary行程_1");

            entity.ToTable("Itinerary");

            entity.Property(e => e.Available).HasMaxLength(50);
            entity.Property(e => e.ItineraryId)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.ItineraryName).IsRequired();
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.ActivitySystem).WithMany(p => p.Itineraries)
                .HasForeignKey(d => d.ActivitySystemId)
                .HasConstraintName("FK_Itinerary_Activity");

            entity.HasOne(d => d.AreaSystem).WithMany(p => p.Itineraries)
                .HasForeignKey(d => d.AreaSystemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Itinerary行程_Area地區");
        });

        modelBuilder.Entity<ItineraryDate>(entity =>
        {
            entity.HasKey(e => e.ItineraryDateSystemId).HasName("PK_ItineraryTime行程批次");

            entity.ToTable("ItineraryDate");

            entity.Property(e => e.DepartureDate).HasMaxLength(50);

            entity.HasOne(d => d.ItinerarySystem).WithMany(p => p.ItineraryDates)
                .HasForeignKey(d => d.ItinerarySystemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItineraryDate_Itinerary");
        });

        modelBuilder.Entity<ItineraryOrder>(entity =>
        {
            entity.HasKey(e => e.ItineraryOrderId).HasName("PK_訂單資料_行程");

            entity.ToTable("ItineraryOrder");

            entity.Property(e => e.ItineraryOrderNumber).HasMaxLength(50);
            entity.Property(e => e.OrderTime).HasColumnType("datetime");
            entity.Property(e => e.PaymentTime).HasColumnType("datetime");
            entity.Property(e => e.Remarks).HasMaxLength(50);
            entity.Property(e => e.RepresentativeFirstName).HasMaxLength(50);
            entity.Property(e => e.RepresentativeIdnumber).HasMaxLength(50);
            entity.Property(e => e.RepresentativeLastName).HasMaxLength(50);
            entity.Property(e => e.RepresentativePassportNumber).HasMaxLength(50);
            entity.Property(e => e.RepresentativePhoneNumber).HasMaxLength(50);
            entity.Property(e => e.ReveiwContent).HasMaxLength(50);
            entity.Property(e => e.ReviewTime).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("money");
        });

        modelBuilder.Entity<ItineraryOrderItem>(entity =>
        {
            entity.ToTable("ItineraryOrderItem");

            entity.Property(e => e.CommentContent).HasMaxLength(50);

            entity.HasOne(d => d.ItineraryDateSystem).WithMany(p => p.ItineraryOrderItems)
                .HasForeignKey(d => d.ItineraryDateSystemId)
                .HasConstraintName("FK_ItineraryOrderItem_ItineraryDate");

            entity.HasOne(d => d.Order).WithMany(p => p.ItineraryOrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_ItineraryOrderItem_Order");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.EnglishName).HasMaxLength(50);
            entity.Property(e => e.ImagePath)
                .HasMaxLength(50)
                .HasColumnName("imagePath");
            entity.Property(e => e.MemberLevelId).HasColumnName("MemberLevelID");
            entity.Property(e => e.MemberName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.MemberStatusId).HasColumnName("MemberStatusID");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);

            entity.HasOne(d => d.City).WithMany(p => p.Members)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_Member_City1");

            entity.HasOne(d => d.MemberLevel).WithMany(p => p.Members)
                .HasForeignKey(d => d.MemberLevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_MemberLevel");

            entity.HasOne(d => d.MemberStatus).WithMany(p => p.Members)
                .HasForeignKey(d => d.MemberStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_MemberStatus");
        });

        modelBuilder.Entity<MemberCouponList>(entity =>
        {
            entity.HasKey(e => e.MemberCouponId).HasName("PK_betwMemberCoupon會員優惠券中繼表");

            entity.ToTable("MemberCouponList");

            entity.Property(e => e.MemberCouponId).HasColumnName("MemberCouponID");
            entity.Property(e => e.CouponId).HasColumnName("CouponID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");

            entity.HasOne(d => d.Coupon).WithMany(p => p.MemberCouponLists)
                .HasForeignKey(d => d.CouponId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_betwMemberCoupon_Coupon優惠券");

            entity.HasOne(d => d.Member).WithMany(p => p.MemberCouponLists)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_betwMemberCoupon會員優惠券中繼表_Member會員");
        });

        modelBuilder.Entity<MemberLevel>(entity =>
        {
            entity.ToTable("MemberLevel");

            entity.Property(e => e.MemberLevelId).HasColumnName("MemberLevelID");
            entity.Property(e => e.Condtition).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MemberLevelName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<MemberStatus>(entity =>
        {
            entity.ToTable("MemberStatus");

            entity.Property(e => e.MemberStatusId).HasColumnName("MemberStatusID");
            entity.Property(e => e.MemberStatusName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<MyCollection>(entity =>
        {
            entity.HasKey(e => e.MyCollectionId).HasName("PK_MyFavorite我的最愛");

            entity.ToTable("MyCollection");

            entity.Property(e => e.MyCollectionId).HasColumnName("MyCollectionID");
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");

            entity.HasOne(d => d.Itinerary).WithMany(p => p.MyCollections)
                .HasForeignKey(d => d.ItineraryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MyFavorite我的最愛_Itinerary行程");

            entity.HasOne(d => d.Member).WithMany(p => p.MyCollections)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MyCollection_Member");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order", tb => tb.HasTrigger("trg_AfterInsertOrder"));

            entity.Property(e => e.OrderNumber).HasMaxLength(50);
            entity.Property(e => e.OrderTime).HasColumnType("datetime");
            entity.Property(e => e.PaymentTime).HasColumnType("datetime");
            entity.Property(e => e.Remarks).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("money");

            entity.HasOne(d => d.Coupon).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CouponId)
                .HasConstraintName("FK_Order_Coupon");

            entity.HasOne(d => d.Member).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_Order_Member");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .HasConstraintName("FK_Order_OrderStatus");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK_Order_PaymentMethod");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId).HasName("PK_訂單狀態");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.OrderStatus1)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("OrderStatus");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK_付款方式");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.PaymentMethod1)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("PaymentMethod");
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.PaymentStatusId).HasName("PK_付款狀態");

            entity.ToTable("PaymentStatus");

            entity.Property(e => e.PaymentStatus1)
                .HasMaxLength(50)
                .HasColumnName("PaymentStatus");
        });

        modelBuilder.Entity<Port>(entity =>
        {
            entity.HasKey(e => e.PortId).HasName("PK_tPort");

            entity.ToTable("Port");

            entity.Property(e => e.PortId).HasColumnName("PortID");
            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PortName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<PortImage>(entity =>
        {
            entity.ToTable("PortImage");

            entity.Property(e => e.PortImageId).HasColumnName("PortImageID");
            entity.Property(e => e.PortId).HasColumnName("PortID");
            entity.Property(e => e.PortImageDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PortImageUrl).HasColumnName("PortImageURL");

            entity.HasOne(d => d.Port).WithMany(p => p.PortImages)
                .HasForeignKey(d => d.PortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PortImage_Port");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("PK_tRoutes");

            entity.ToTable("Route");

            entity.Property(e => e.RouteId).HasColumnName("RouteID");
            entity.Property(e => e.DestinationPortId).HasColumnName("DestinationPortID");
            entity.Property(e => e.OriginPortId).HasColumnName("OriginPortID");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.RouteDescription).HasColumnType("text");

            entity.HasOne(d => d.DestinationPort).WithMany(p => p.RouteDestinationPorts)
                .HasForeignKey(d => d.DestinationPortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tRoutes_tPort1");

            entity.HasOne(d => d.OriginPort).WithMany(p => p.RouteOriginPorts)
                .HasForeignKey(d => d.OriginPortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tRoutes_tPort");
        });

        modelBuilder.Entity<RouteImage>(entity =>
        {
            entity.HasKey(e => e.RouteImageId).HasName("PK_tImages");

            entity.ToTable("RouteImage");

            entity.Property(e => e.RouteImageId).HasColumnName("RouteImageID");
            entity.Property(e => e.RouteId).HasColumnName("RouteID");
            entity.Property(e => e.RouteImageDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RouteImageUrl).HasColumnName("RouteImageURL");

            entity.HasOne(d => d.Route).WithMany(p => p.RouteImages)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tImages_tRoutes");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK_tSchedules");

            entity.ToTable("Schedule");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.ArrivalTime).HasColumnType("datetime");
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");
            entity.Property(e => e.RouteId).HasColumnName("RouteID");

            entity.HasOne(d => d.Route).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tSchedules_tRoutes");
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.HasKey(e => e.ThemeSystemId).HasName("PK_Theme主題");

            entity.ToTable("Theme");

            entity.Property(e => e.ThemeName).HasMaxLength(50);
        });

        modelBuilder.Entity<TicketOrder>(entity =>
        {
            entity.HasKey(e => e.TicketOrderId).HasName("PK_訂單資料_航班");

            entity.ToTable("TicketOrder");

            entity.Property(e => e.OrderTime).HasColumnType("datetime");
            entity.Property(e => e.PaymentTime).HasColumnType("datetime");
            entity.Property(e => e.Remarks).HasMaxLength(50);
            entity.Property(e => e.RepresentativeFirstName).HasMaxLength(50);
            entity.Property(e => e.RepresentativeIdnumber).HasMaxLength(50);
            entity.Property(e => e.RepresentativeLastName).HasMaxLength(50);
            entity.Property(e => e.RepresentativePassportNumber).HasMaxLength(50);
            entity.Property(e => e.RepresentativePhoneNumber).HasMaxLength(50);
            entity.Property(e => e.ReviewContent).HasMaxLength(50);
            entity.Property(e => e.ReviewTime).HasColumnType("datetime");
            entity.Property(e => e.TicketOrderNumber).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("money");
        });

        modelBuilder.Entity<TicketOrderItem>(entity =>
        {
            entity.HasKey(e => e.TicketOrderItemId).HasName("PK_航班訂單Detail");

            entity.ToTable("TicketOrderItem");

            entity.Property(e => e.CommentContent).HasMaxLength(50);
            entity.Property(e => e.CommentTime).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.TicketOrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketOrderItem_Order");

            entity.HasOne(d => d.Schedule).WithMany(p => p.TicketOrderItems)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_航班訂單Detail_渡輪航班1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}