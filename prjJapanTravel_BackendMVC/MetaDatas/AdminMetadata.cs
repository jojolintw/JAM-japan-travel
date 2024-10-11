using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prjJapanTravel_BackendMVC.MetaDatas
{
    public class AdminMetadata
    {
        [Key]
        [Column("AdminID")]
        public int AdminId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name ="管理員姓名")]
        public string AdminName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Column("imagePath")]
        [StringLength(50)]
        public string ImagePath { get; set; }
        [Display(Name = "管理員管理權限")]
        public bool AdminManageStatus { get; set; }
        [Display(Name = "會員管理權限")]
        public bool MemberManageStatus { get; set; }
        [Display(Name = "行程管理權限")]
        public bool IniteraryManageStatus { get; set; }
        [Display(Name = "傳票管理權限")]
        public bool ShipmentManageStatus { get; set; }
        [Display(Name = "訂單管理權限")]
        public bool OrderManageStatus { get; set; }
        [Display(Name = "優惠券管理權限")]
        public bool CouponManageStatus { get; set; }
        [Display(Name = "評論管理權限")]
        public bool CommentManageStatus { get; set; }
        [Display(Name = "部落格管理權限")]
        public bool BlogManageStatus { get; set; }
    }
}
