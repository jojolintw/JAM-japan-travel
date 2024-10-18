namespace prjJapanTravel_BackendMVC.ViewModels
{
    public class RouteDetailsViewModel
    {
        public int RouteId { get; set; }
        public string OriginPortName { get; set; }
        public string DestinationPortName { get; set; }
        public decimal Price { get; set; }
        public string RouteDescription { get; set; }
        public List<ScheduleViewModel> Schedules { get; set; }
        public List<RouteImageViewModel> RouteImages { get; set; }
    }

    public class ScheduleViewModel
    {
        public int ScheduleId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int Seats { get; set; }
        public int Capacity { get; set; }
    }

    public class RouteImageViewModel
    {
        public int RouteImageId { get; set; }
        public string RouteImageBase64 { get; set; } // 用來存儲轉換後的Base64圖片字串
        public string RouteImageDescription { get; set; }
    }

}
