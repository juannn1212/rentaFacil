using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicleService.Domain.Entities;
using VehicleService.Domain.Repositories;

namespace VehicleService.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleDbContext _ctx;
        public VehicleRepository(VehicleDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Vehicle>> GetAllAsync() =>
            await _ctx.Vehicles.ToListAsync();

        public async Task<Vehicle?> GetByIdAsync(Guid id) =>
            await _ctx.Vehicles.FindAsync(id);

        public async Task AddAsync(Vehicle vehicle) =>
            await _ctx.Vehicles.AddAsync(vehicle);

        public void Update(Vehicle vehicle) =>
            _ctx.Vehicles.Update(vehicle);

        public void Delete(Vehicle vehicle) =>
            _ctx.Vehicles.Remove(vehicle);

        public Task<int> SaveChangesAsync() =>
            _ctx.SaveChangesAsync();

        public async Task<IEnumerable<Vehicle>> GetAvailableAsync(string? type, DateTime? date)
        {
            var query = _ctx.Vehicles
                .Where(v => v.IsAvailable)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(v => v.Type == type);
            }

            if (date.HasValue)
            {
                var dt = date.Value.Date;
                query = query.Where(v =>
                    !_ctx.Bookings.Any(b =>
                        b.VehicleId == v.Id &&
                        b.StartDate.Date <= dt &&
                        b.EndDate.Date >= dt));
            }

            return await query.ToListAsync();
        }
    }
}
