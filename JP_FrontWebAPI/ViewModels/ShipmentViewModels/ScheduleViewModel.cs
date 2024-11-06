public class ScheduleViewModel
{
    public int ScheduleId { get; set; }
    public DateTime DepartureTime { get; set; }

    public int RouteId { get; set; }


    public DateTime ArrivalTime { get; set; }

    public int Seats { get; set; }

    public int Capacity { get; set; }

    public virtual Route Route { get; set; }

}
