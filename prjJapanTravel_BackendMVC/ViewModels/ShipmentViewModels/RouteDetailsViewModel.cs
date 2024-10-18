using prjJapanTravel_BackendMVC.Models;

namespace prjJapanTravel_BackendMVC.ViewModels.ShipmentViewModels
{
    public class RouteDetailsViewModel
    {
        public Models.Route Route { get; set; }
        public IEnumerable<Schedule> Schedules { get; set; }
        public IEnumerable<RouteImage> RouteImages { get; set; }

        // 用於新增圖片
        public RouteImage NewRouteImage { get; set; } = new RouteImage();

        // 用於新增排班
        public Schedule NewSchedule { get; set; } = new Schedule();
    }
}
