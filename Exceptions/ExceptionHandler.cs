using inventory_management_system.Constants;
using inventory_management_system.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security;

namespace inventory_management_system.Exceptions
{
    public static class ExceptionHandler
    {
        public static ObjectResult HandleException(Exception exception, HttpContext httpContext)
        {
            var (errorKey, statusCode, title) = MapExceptionToError(exception);

            var errorResponse = ProblemDetailHelper.CreateErrorResponse(
                errorKey: errorKey,
                title: title,
                status: statusCode,
                detail: GetSafeErrorMessage(exception, statusCode),
                httpContext: httpContext
            );

            return new ObjectResult(errorResponse)
            {
                StatusCode = statusCode,
                ContentTypes = { "application/problem+json" }
            };
        }

        private static (string ErrorKey, int StatusCode, string Title) MapExceptionToError(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException or ArgumentException =>
                    (ErrorConstants.BAD_REQUEST,
                     StatusCodes.Status400BadRequest,
                     ErrorConstants.Titles.INVALID_INPUT),

                UnauthorizedAccessException =>
                    (ErrorConstants.UNAUTHORIZED,
                     StatusCodes.Status401Unauthorized,
                     ErrorConstants.Titles.USER_NOT_AUTHORIZED),

                SecurityException =>
                    (ErrorConstants.FORBIDDEN,
                     StatusCodes.Status403Forbidden,
                     ErrorConstants.Titles.ACCESS_FORBIDDEN),

                KeyNotFoundException =>
                    (ErrorConstants.NOT_FOUND,
                     StatusCodes.Status404NotFound,
                     ErrorConstants.Titles.RESOURCE_NOT_FOUND),

                TimeoutException =>
                    (ErrorConstants.TIMEOUT,
                     StatusCodes.Status408RequestTimeout,
                     ErrorConstants.Titles.REQUEST_TIMEOUT),

                InvalidOperationException =>
                    (ErrorConstants.CONFLICT,
                     StatusCodes.Status409Conflict,
                     ErrorConstants.Titles.REQUEST_CONFLICT),

                ApplicationException =>
                    (ErrorConstants.UNPROCESSABLE_ENTITY,
                     StatusCodes.Status422UnprocessableEntity,
                     ErrorConstants.Titles.BUSINESS_RULE_FAILED),

                SqlException =>
                    (ErrorConstants.DATABASE_ERROR,
                     StatusCodes.Status500InternalServerError,
                     ErrorConstants.Titles.DATABASE_ERROR),

                _ =>
                    (ErrorConstants.INTERNAL_SERVER_ERROR,
                     StatusCodes.Status500InternalServerError,
                     ErrorConstants.Titles.UNEXPECTED_ERROR)
            };
        }

        private static string GetSafeErrorMessage(Exception exception, int statusCode)
        {
            //  5xx errors
            if (statusCode >= 500)
            {
                return statusCode switch
                {
                    StatusCodes.Status500InternalServerError =>
                        "An unexpected error occurred. Please contact support if the issue persists.",
                    _ =>
                        "A server error occurred. Please try again later."
                };
            }

            // For client errors (4xx)
            return exception.Message;
        }
    }
}