// BookingService.Domain/Entities/Client.cs
namespace BookingService.Domain.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
    }
}
