// VehicleService.Application/Services/IVehicleService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleService.Domain.Entities;

namespace VehicleService.Application.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task<Vehicle> CreateAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Vehicle>> GetAvailableAsync(string? type, DateTime? date);
        

    }
}
