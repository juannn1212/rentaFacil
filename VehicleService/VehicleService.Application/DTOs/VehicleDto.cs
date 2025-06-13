using System.ComponentModel.DataAnnotations;

namespace VehicleService.Application.DTOs
{
    public class VehicleDto
    {
        public Guid Id { get; set; }
        public string LicensePlate { get; set; } = null!;
        public string Type { get; set; } = null!;
        public bool IsAvailable { get; set; }
    }
}
