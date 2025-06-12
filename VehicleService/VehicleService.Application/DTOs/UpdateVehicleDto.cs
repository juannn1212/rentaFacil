// VehicleService.Application/DTOs/UpdateVehicleDto.cs
using System.ComponentModel.DataAnnotations;
namespace VehicleService.Application.DTOs
{
    public class UpdateVehicleDto
    {
        [Required]
        public string Type { get; set; } = null!;

        public bool IsAvailable { get; set; }
    }
}