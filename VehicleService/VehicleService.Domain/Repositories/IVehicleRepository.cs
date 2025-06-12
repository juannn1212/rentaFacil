// VehicleService.Domain/Repositories/IVehicleRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleService.Domain.Entities;

namespace VehicleService.Domain.Repositories
{
    public interface IVehicleRepository   // <- aquí debe decir public
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task AddAsync(Vehicle vehicle);
        void Update(Vehicle vehicle);
        void Delete(Vehicle vehicle);
        Task<int> SaveChangesAsync();
        Task<IEnumerable<Vehicle>> GetAvailableAsync(string? type, DateTime? date);

    }
}
