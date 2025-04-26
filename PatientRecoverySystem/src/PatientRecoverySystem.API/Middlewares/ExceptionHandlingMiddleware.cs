using System.Net;
using System.Text.Json;

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception");

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = context.Response;
            var result = "";

            switch (exception)
            {
                case InvalidOperationException invalidOperation:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { message = invalidOperation.Message });
                    break;

                case UnauthorizedAccessException unauthorized:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    result = JsonSerializer.Serialize(new { message = unauthorized.Message });
                    break;

                case KeyNotFoundException notFound:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new { message = notFound.Message });
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new { message = "An unexpected error occurred." });
                    break;
            }

            return context.Response.WriteAsync(result);
        }
    }
}
