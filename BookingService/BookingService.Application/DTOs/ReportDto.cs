// BookingService.Application/DTOs/ReportDto.cs
namespace BookingService.Application.DTOs
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int TotalBookings { get; set; }
        public int TotalVehiclesUsed { get; set; }
    }
}
