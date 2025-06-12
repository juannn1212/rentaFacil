// VehicleService.Application/Services/VehicleService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleService.Domain.Entities;
using VehicleService.Domain.Repositories;

namespace VehicleService.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repo;
        public VehicleService(IVehicleRepository repo) => _repo = repo;

        public async Task<IEnumerable<Vehicle>> GetAllAsync() =>
            await _repo.GetAllAsync();

        public async Task<Vehicle?> GetByIdAsync(Guid id) =>
            await _repo.GetByIdAsync(id);

        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            vehicle.Id = Guid.NewGuid();
            await _repo.AddAsync(vehicle);
            await _repo.SaveChangesAsync();
            return vehicle;
        }

        public Task<IEnumerable<Vehicle>> GetAvailableAsync(string? type, DateTime? date)
    => _repo.GetAvailableAsync(type, date);


        public async Task UpdateAsync(Vehicle vehicle)
        {
            _repo.Update(vehicle);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var v = await _repo.GetByIdAsync(id);
            if (v is null) throw new KeyNotFoundException("Vehicle not found");
            _repo.Delete(v);
            await _repo.SaveChangesAsync();
        }
    }
}
