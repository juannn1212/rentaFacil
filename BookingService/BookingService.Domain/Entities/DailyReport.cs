// BookingService.Domain/Entities/DailyReport.cs
namespace BookingService.Domain.Entities
{
    public class DailyReport
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int TotalBookings { get; set; }
        public int TotalVehiclesUsed { get; set; }
    }
}
