// BookingService.Infrastructure/Data/BookingDbInitializer.cs

using System;
using System.Linq;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BookingService.Domain.Entities;
using BookingService.Infrastructure;  // Para BookingDbContext

namespace BookingService.Infrastructure.Data
{
    public static class BookingDbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BookingDbContext>();

            context.Database.Migrate();

            if (!context.Clients.Any())
            {
                var clients = new[]
                {
                    new Client
                    {
                        Id = Guid.NewGuid(),
                        Name = "Juan Pérez",
                        Email = "juan.perez@example.com",
                        Phone = "3001234567"
                    },
                    new Client
                    {
                        Id = Guid.NewGuid(),
                        Name = "María Gómez",
                        Email = "maria.gomez@example.com",
                        Phone = "3117654321"
                    }
                };

                context.Clients.AddRange(clients);
                context.SaveChanges();
            }

            Guid? vehicleId = null;
            var conn = context.Database.GetDbConnection();
            try
            {
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT TOP 1 Id FROM Vehicles";
                var result = cmd.ExecuteScalar();
                if (result is Guid g) vehicleId = g;
            }
            finally
            {
                conn.Close();
            }

            if (vehicleId.HasValue && !context.Bookings.Any())
            {
                var firstClientId = context.Clients.OrderBy(c => c.Name).First().Id;

                var exampleBooking = new Booking
                {
                    Id = Guid.NewGuid(),
                    VehicleId = vehicleId.Value,
                    ClientId = firstClientId,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(3),
                    Status = "Pending"
                };

                context.Bookings.Add(exampleBooking);
                context.SaveChanges();
            }
        }
    }
}
