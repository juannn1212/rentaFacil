// BookingService.Application/Services/IBookingService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookingService.Domain.Entities;

namespace BookingService.Application.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(Guid id);
        Task<Booking> CreateAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(Guid id);

        Task<IEnumerable<Booking>> GetByClientIdAsync(Guid clientId);

    }
}
