// Program.cs
using System;
using BookingService.Application.Services;
using BookingService.Domain.Repositories;            // IBookingRepository
using BookingService.Infrastructure;                // BookingDbContext
using BookingService.Infrastructure.Data;           // BookingDbInitializer (opcional)
using BookingService.Infrastructure.Repositories;   // BookingRepository
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Alias para la implementación de IBookingService
using BkSvc = BookingService.Application.Services.BookingService;

var builder = Host.CreateApplicationBuilder(args);

// 1️⃣ BookingDbContext (reservas)
var bookingConn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookingDbContext>(opt =>
    opt.UseSqlServer(bookingConn, sqlOpts =>
        sqlOpts.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
    )
);

// 2️⃣ Repositorio y servicio de Booking
builder.Services
    .AddScoped<IBookingRepository, BookingRepository>()
    .AddScoped<IBookingService, BkSvc>();

// 3️⃣ ReportingDbContext (reportes)
builder.Services.AddDbContext<ReportingDbContext>(opt =>
    opt.UseSqlServer(bookingConn)
);

// 4️⃣ Hosted Service (Worker)
builder.Services.AddHostedService<Worker.Worker>();

var host = builder.Build();

// (Opcional) Seed de Bookings: BookingDbInitializer.Initialize(host.Services);

host.Run();
