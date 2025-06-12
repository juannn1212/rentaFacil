using System;

public class DailyReport
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int TotalBookings { get; set; }
    public int TotalVehiclesUsed { get; set; }
}
