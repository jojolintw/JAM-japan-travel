namespace prjJapanTravel_BackendMVC.ViewModels.AdminViewModels
{
    public class AKeywordViewModel
    {
        public string txtKeyword { get; set; }
        public DateTime? StartDate { get; set; } // 可选的起始日期
        public DateTime? EndDate { get; set; }   // 可选的结束日期
    }
}
