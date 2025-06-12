using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using VehicleService.Application.Mappings;
using VehicleService.Application.Services;
using VehicleService.Domain.Repositories;
using VehicleService.Infrastructure;
using VehicleService.Infrastructure.Data;
using VehicleService.Infrastructure.Repositories;

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
builder.Services.AddDbContext<VehicleDbContext>(opt =>
    opt.UseSqlServer(conn, sqlOpts =>
        sqlOpts.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
    )
);

// Repositorios y servicios
builder.Services
    .AddScoped<IVehicleRepository, VehicleRepository>()
    .AddScoped<IVehicleService, VehicleService.Application.Services.VehicleService>();

// AutoMapper
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<VehicleProfile>();
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// MVC / Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed
DbInitializer.Initialize(app.Services);

// Middleware
app.UseCors("AllowAngularDev");
app.UseMiddleware<VehicleService.API.Middleware.ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
