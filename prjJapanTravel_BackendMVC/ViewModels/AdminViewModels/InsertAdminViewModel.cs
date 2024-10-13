namespace prjJapanTravel_BackendMVC.ViewModels.AdminViewModels
{
    public class InsertAdminViewModel
    {

        public string AdminName { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        ////public string? ImagePath { get; set; }

        public bool AdminManageStatus { get; set; }

        public bool MemberManageStatus { get; set; }

        public bool IniteraryManageStatus { get; set; }

        public bool ShipmentManageStatus { get; set; }

        public bool OrderManageStatus { get; set; }

        public bool CouponManageStatus { get; set; }

        public bool CommentManageStatus { get; set; }

        public bool BlogManageStatus { get; set; }
    }
}
