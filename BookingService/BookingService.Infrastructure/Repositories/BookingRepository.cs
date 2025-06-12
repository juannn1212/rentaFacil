// BookingService.Infrastructure/Repositories/BookingRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Repositories;

namespace BookingService.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _ctx;
        public BookingRepository(BookingDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Booking>> GetAllAsync() =>
            await _ctx.Bookings.ToListAsync();

        public async Task<Booking?> GetByIdAsync(Guid id) =>
            await _ctx.Bookings.FindAsync(id);

        public async Task AddAsync(Booking booking) =>
            await _ctx.Bookings.AddAsync(booking);

        public void Update(Booking booking) =>
            _ctx.Bookings.Update(booking);

        public void Delete(Booking booking) =>
            _ctx.Bookings.Remove(booking);

        public Task<int> SaveChangesAsync() =>
            _ctx.SaveChangesAsync();

        public async Task<IEnumerable<Booking>> GetByClientIdAsync(Guid clientId) =>
            await _ctx.Bookings
                .Where(b => b.ClientId == clientId)
                .ToListAsync();
    }
}
