using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PatientRecoverySystem.Application.Exceptions;
using System.Threading.Tasks;
using System;

namespace PatientRecoverySystem.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (NotFoundException nfEx)
            {
                _logger.LogWarning(nfEx, "Not Found");

                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                    title = "Resource Not Found",
                    status = 404,
                    errors = new { resource = new[] { nfEx.Message } }
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                    title = "An unexpected error occurred.",
                    status = 500,
                    traceId = context.TraceIdentifier
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
