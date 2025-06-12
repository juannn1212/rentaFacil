// VehicleService.Infrastructure/Data/DbInitializer.cs

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VehicleService.Domain.Entities;
using VehicleService.Infrastructure;    // Aquí está tu VehicleDbContext

namespace VehicleService.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<VehicleDbContext>();
            Console.WriteLine($"[Seed] Vehicles in DB: {context.Vehicles.Count()}");


            // 1. Aplica migraciones pendientes
            context.Database.Migrate();

            // 2. Si ya hay datos, no hacemos nada
            if (context.Vehicles.Any())
                return;

            // 3. Semilla de datos de ejemplo
            var vehicles = new[]
            {
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = "ABC-123",
                    Type = "Sedan",
                    IsAvailable = true
                },
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = "DEF-456",
                    Type = "SUV",
                    IsAvailable = true
                },
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = "GHI-789",
                    Type = "Hatchback",
                    IsAvailable = false
                }
            };

            context.Vehicles.AddRange(vehicles);
            context.SaveChanges();
        }
    }
}
