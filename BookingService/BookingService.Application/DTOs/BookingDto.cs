// BookingService.Application/DTOs/BookingDto.cs
using System.ComponentModel.DataAnnotations;

namespace BookingService.Application.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
