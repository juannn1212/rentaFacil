using System;

namespace VehicleService.Domain.Entities
{
    public class Booking
    {
        public Guid VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
