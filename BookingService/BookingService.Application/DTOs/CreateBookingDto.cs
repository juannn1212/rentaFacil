using System.ComponentModel.DataAnnotations;
namespace BookingService.Application.DTOs
{
    public class CreateBookingDto
    {
        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}