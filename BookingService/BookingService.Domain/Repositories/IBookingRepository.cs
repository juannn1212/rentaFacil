using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookingService.Domain.Entities;

namespace BookingService.Domain.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(Guid id);
        Task AddAsync(Booking booking);
        void Update(Booking booking);
        void Delete(Booking booking);
        Task<int> SaveChangesAsync();
        Task<IEnumerable<Booking>> GetByClientIdAsync(Guid clientId);

    }
}
