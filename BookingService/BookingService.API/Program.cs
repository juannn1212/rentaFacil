using AutoMapper;
using BookingService.Application.Mappings;
using BookingService.Application.Services;
using BookingService.Domain.Repositories;
using BookingService.Infrastructure;
using BookingService.Infrastructure.Data;
using BookingService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAngularDev", p => p
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

// DbContext
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookingDbContext>(opt =>
    opt.UseSqlServer(conn, sqlOpts =>
        sqlOpts.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
    )
);

// Repositorios y servicios
builder.Services
    .AddScoped<IBookingRepository, BookingRepository>()
    .AddScoped<IDailyReportRepository, DailyReportRepository>()
    .AddScoped<IBookingService, BookingService.Application.Services.BookingService>()
    .AddScoped<IDailyReportService, DailyReportService>();

// AutoMapper
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<BookingProfile>();
    cfg.AddProfile<ReportProfile>();
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// MVC / Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed
BookingDbInitializer.Initialize(app.Services);

// Middleware
app.UseCors("AllowAngularDev");
app.UseMiddleware<BookingService.API.Middleware.ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
