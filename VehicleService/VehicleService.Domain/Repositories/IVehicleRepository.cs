using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleService.Domain.Entities;

namespace VehicleService.Domain.Repositories
{
    public interface IVehicleRepository  
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
