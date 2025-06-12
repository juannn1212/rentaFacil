// BookingService.Domain/Repositories/IDailyReportRepository.cs
using BookingService.Domain.Entities;

namespace BookingService.Domain.Repositories
{
    public interface IDailyReportRepository
    {
        Task AddAsync(DailyReport report);
        Task<int> SaveChangesAsync();
    }
}
