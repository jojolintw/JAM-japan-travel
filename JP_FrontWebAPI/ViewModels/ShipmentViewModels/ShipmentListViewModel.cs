
namespace prjJapanTravel_BackendMVC.ViewModels.ShipmentViewModels
{
    public class ShipmentListViewModel
    {
        public int RouteId { get; set; }
        public string OriginPortName { get; set; }
        public string DestinationPortName { get; set; }
        public decimal Price { get; set; }
        public string RouteDescription { get; set; }
        public string NextDeparture { get; set; } // 將 DateTime? 改為 string
    }

}
