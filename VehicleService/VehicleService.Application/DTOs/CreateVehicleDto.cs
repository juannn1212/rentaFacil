using System.ComponentModel.DataAnnotations;
namespace VehicleService.Application.DTOs
{
    public class CreateVehicleDto
    {
        [Required]
        public string LicensePlate { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;
    }
}