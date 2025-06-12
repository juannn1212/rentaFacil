using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BookingService.Application.Services;

namespace Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker iniciado a las: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                // 1. Esperar hasta la próxima medianoche local 
                var now = DateTime.Now;
                var nextRun = now.Date.AddDays(1); 
                var delay = nextRun - now;

                _logger.LogInformation("Próxima ejecución programada para: {nextRun}", nextRun);
                await Task.Delay(delay, stoppingToken);

                // 2. Generar reporte para el día anterior
                var date = nextRun.Date;
                _logger.LogInformation("Generando reporte de reservas para: {date}", date);

                // Crear un scope para servicios scoped
                using var scope = _scopeFactory.CreateScope();
                var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();

                // 3. Obtener y filtrar reservas del día
                var bookings = await bookingService.GetAllAsync();
                var todays = bookings.Where(b => b.StartDate.Date == date).ToList();

                // 4. Calcular métricas
                var totalBookings = todays.Count;
                var totalUniqueVehicles = todays.Select(b => b.VehicleId).Distinct().Count();

                // 5. Log del reporte
                _logger.LogInformation(
                    "Reporte {date}: TotalBookings={total}, TotalVehicles={vehicles}",
                    date, totalBookings, totalUniqueVehicles
                );

                // 6. Guardar reporte en CSV en la carpeta de la aplicación
                var filePath = Path.Combine(AppContext.BaseDirectory, "DailyReport.csv");
                var writeHeader = !File.Exists(filePath);
                using var sw = new StreamWriter(filePath, append: true);
                if (writeHeader)
                    sw.WriteLine("Date,TotalBookings,TotalVehiclesUsed");
                sw.WriteLine($"{date:yyyy-MM-dd},{totalBookings},{totalUniqueVehicles}");
                _logger.LogInformation("Reporte también guardado en archivo: {file}", filePath);
            }
        }
    }
}
