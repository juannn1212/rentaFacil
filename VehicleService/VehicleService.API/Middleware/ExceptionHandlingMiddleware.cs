using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using VehicleService.Domain.Exceptions;
using VehicleService.Application.Exceptions;

namespace VehicleService.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _log;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> log)
        {
            _next = next;
            _log = log;
        }

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (BusinessException ex)
            {
                _log.LogWarning(ex, "Regla de negocio violada");
                ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
                ctx.Response.ContentType = "application/json";
                var payload = new { error = ex.Message };
                await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
            catch (ValidationException ex)
            {
                _log.LogWarning(ex, "Validación de modelo fallida");
                ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
                ctx.Response.ContentType = "application/json";
                var payload = new { errors = ex.Errors };
                await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error interno inesperado");
                ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
                ctx.Response.ContentType = "application/json";
                var payload = new { error = "Ha ocurrido un error interno." };
                await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        }
    }
}
