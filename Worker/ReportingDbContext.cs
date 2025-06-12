using Microsoft.EntityFrameworkCore;

public class ReportingDbContext : DbContext
{
    public ReportingDbContext(DbContextOptions<ReportingDbContext> options)
        : base(options) { }

    public DbSet<DailyReport> DailyReports { get; set; }
}
