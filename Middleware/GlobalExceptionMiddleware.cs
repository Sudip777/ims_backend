using FluentValidation;
using inventory_management_system.Exceptions;

namespace inventory_management_system.Middleware
{
   
        public class GlobalExceptionMiddleware
        {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException validationException)
            {
                await HandleValidationExceptionAsync(context, validationException);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleValidationExceptionAsync(
            HttpContext context,
            ValidationException validationException)
        {
            _logger.LogWarning(
                validationException,
                "Validation failed for {Path}. Error count: {ErrorCount}",
                context.Request.Path,
                validationException.Errors.Count());

            var result = Helpers.ProblemDetailHelper.CreateValidationErrorObjectResult(
                new FluentValidation.Results.ValidationResult(validationException.Errors),
                context);

            context.Response.StatusCode = result.StatusCode ?? 400;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(result.Value);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = GetStatusCode(exception);

            if (statusCode >= 500)
            {
                _logger.LogError(
                    exception,
                    "Server error occurred. Path: {Path}, Method: {Method}, TraceId: {TraceId}",
                    context.Request.Path,
                    context.Request.Method,
                    context.TraceIdentifier);
            }
            else
            {
                _logger.LogWarning(
                    exception,
                    "Client error occurred. Path: {Path}, Method: {Method}, StatusCode: {StatusCode}",
                    context.Request.Path,
                    context.Request.Method,
                    statusCode);
            }

            var result = ExceptionHandler.HandleException(exception, context);

            context.Response.StatusCode = result.StatusCode ?? 500;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(result.Value);
        }

        private static int GetStatusCode(Exception exception) => exception switch
        {
            ArgumentNullException or ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            System.Security.SecurityException => StatusCodes.Status403Forbidden,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            TimeoutException => StatusCodes.Status408RequestTimeout,
            InvalidOperationException => StatusCodes.Status409Conflict,
            ApplicationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };
    }
    
}
