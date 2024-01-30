using System.Net;
using System.Text.Json;

namespace Payments.Presentacion.Middlewares
{
    public class ExceptionHandlerMiddleware(RequestDelegate next, bool showRawError, ILogger<ExceptionHandlerMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly bool _showRawError = showRawError;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                await HandleExceptionAsync(context, error);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            _logger.LogError(error, error.Message);
            string _error = _showRawError ? error.Message : "";
            string _message = "Internal Server Error";

            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new { message = _message, error = _error });
            await response.WriteAsync(result);
        }
    }
}
