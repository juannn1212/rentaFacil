using BookingService.Domain.Entities;
using BookingService.Domain.Repositories;

namespace BookingService.Infrastructure.Repositories
{
    public class DailyReportRepository : IDailyReportRepository
    {
        private readonly BookingDbContext _ctx;
        public DailyReportRepository(BookingDbContext ctx) => _ctx = ctx;

        public async Task AddAsync(DailyReport report) =>
            await _ctx.DailyReports.AddAsync(report);

        public Task<int> SaveChangesAsync() =>
            _ctx.SaveChangesAsync();
    }
}
