public class ShipmentDetailViewModel
{
    public int RouteId { get; set; }
    public string OriginPortName { get; set; }
    public string DestinationPortName { get; set; }
    public decimal Price { get; set; }
    public string RouteDescription { get; set; }
    public PortDetailViewModel OriginPort { get; set; }
    public PortDetailViewModel DestinationPort { get; set; }

}

public class PortDetailViewModel
{
    public int PortId { get; set; }
    public string PortName { get; set; }
    public string City { get; set; }
    public string CityDescription1 { get; set; }
    public string CityDescription2 { get; set; }
    public string PortGoogleMap { get; set; }


}
