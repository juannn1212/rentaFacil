using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookingService.Application.Services
{
    public interface IDailyReportService
    {
        Task GenerateReportForDateAsync(DateTime date, CancellationToken token);
    }
}
