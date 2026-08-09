using JobTracker.Application.Common.Exceptions;
using System.Text.Json;

namespace JobTracker.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case ValidationException validationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    var validationResponse = new
                    {
                        message = "Validation failed",
                        errors = validationException.Errors
                    };

                    await context.Response.WriteAsJsonAsync(validationResponse);
                    break;

                default:

                    _logger.LogError(exception, "Unhandled exception occurred");

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    var errorResponse = new
                    {
                        message = "An unexpected error occurred"
                    };

                    await context.Response.WriteAsJsonAsync(errorResponse);

                    break;
            }
        }
    }
}
