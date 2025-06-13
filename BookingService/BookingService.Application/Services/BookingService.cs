using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookingService.Domain.Entities;
using BookingService.Domain.Repositories;

namespace BookingService.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;

        public BookingService(IBookingRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Booking>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<Booking?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public async Task<Booking> CreateAsync(Booking booking)
        {
            booking.Id = Guid.NewGuid();
            await _repo.AddAsync(booking);
            await _repo.SaveChangesAsync();
            return booking;
        }

        public Task<IEnumerable<Booking>> GetByClientIdAsync(Guid clientId)
            => _repo.GetByClientIdAsync(clientId);

        public async Task UpdateAsync(Booking booking)
        {
            _repo.Update(booking);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var booking = await _repo.GetByIdAsync(id);
            if (booking is null)
                throw new KeyNotFoundException($"Booking with id '{id}' not found.");

            _repo.Delete(booking);
            await _repo.SaveChangesAsync();
        }
    }
}
