using Microsoft.EntityFrameworkCore;
using VehicleService.Domain.Entities;

namespace VehicleService.Infrastructure
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options)
            : base(options) { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>(b =>
            {
                b.HasNoKey();
                b.ToTable("Bookings", t => t.ExcludeFromMigrations());

                b.Property<DateTime>("StartDate").HasColumnName("StartDate");
                b.Property<DateTime>("EndDate").HasColumnName("EndDate");
                b.Property<Guid>("VehicleId").HasColumnName("VehicleId");
            });
        }
    }
}
