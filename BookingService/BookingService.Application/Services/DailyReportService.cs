// BookingService.Application/Services/DailyReportService.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookingService.Domain.Entities;
using BookingService.Domain.Repositories;

namespace BookingService.Application.Services
{
    public class DailyReportService : IDailyReportService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IDailyReportRepository _reportRepo;

        public DailyReportService(
            IBookingRepository bookingRepo,
            IDailyReportRepository reportRepo)
        {
            _bookingRepo = bookingRepo;
            _reportRepo = reportRepo;
        }

        public async Task GenerateReportForDateAsync(DateTime date, CancellationToken token)
        {
            var allBookings = await _bookingRepo.GetAllAsync();
            var todays = allBookings.Where(b => b.StartDate.Date == date.Date);

            var report = new DailyReport
            {
                Id = Guid.NewGuid(),
                Date = date.Date,
                TotalBookings = todays.Count(),
                TotalVehiclesUsed = todays
                    .Select(b => b.VehicleId)
                    .Distinct()
                    .Count()
            };

            await _reportRepo.AddAsync(report);
            await _reportRepo.SaveChangesAsync();
        }
    }
}
