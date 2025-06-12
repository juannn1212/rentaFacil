// VehicleService.Domain/Entities/Vehicle.cs
namespace VehicleService.Domain.Entities
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string LicensePlate { get; set; } = null!;
        public string Type { get; set; } = null!;
        public bool IsAvailable { get; set; } = true;
    }
}
