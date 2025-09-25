using Microsoft.Data.SqlClient;
using System.Security;

namespace inventory_management_system.Helpers
{
    public class ExceptionHandler
    {
        public static class ErrorHandler
        {

            public static ErrorResponse HandleException(Exception exception, HttpContext httpContext)
            {

                string errorKey;
                int statusCode;
                string title;

                // Map exception types to error details dynamically
                switch (exception)
                {
                    case ArgumentNullException or ArgumentException:
                        errorKey = "BadRequest";
                        statusCode = StatusCodes.Status400BadRequest;
                        title = "Invalid input parameters.";
                        break;

                    case UnauthorizedAccessException:
                        errorKey = "Unauthorized";
                        statusCode = StatusCodes.Status401Unauthorized;
                        title = "User is not authorized.";
                        break;

                    case SecurityException:
                        errorKey = "Forbidden";
                        statusCode = StatusCodes.Status403Forbidden;
                        title = "Access to this resource is forbidden.";
                        break;

                    case KeyNotFoundException:
                        errorKey = "NotFound";
                        statusCode = StatusCodes.Status404NotFound;
                        title = "The requested resource was not found.";
                        break;

                    case InvalidOperationException:
                        errorKey = "Conflict";
                        statusCode = StatusCodes.Status409Conflict;
                        title = "The request conflicts with the current state.";
                        break;
                    case SqlException:
                        errorKey = "DatabaseError";
                        statusCode = StatusCodes.Status500InternalServerError;
                        title = "A database error occurred.";
                        break;

                    case TimeoutException:
                        errorKey = "Timeout";
                        statusCode = StatusCodes.Status408RequestTimeout;
                        title = "The request timed out while processing.";
                        break;
                    case ApplicationException:
                        errorKey = "UnprocessableEntity";
                        statusCode = StatusCodes.Status422UnprocessableEntity;
                        title = "Business rule validation failed.";
                        break;

                    default:
                        errorKey = "InternalServerError";
                        statusCode = StatusCodes.Status500InternalServerError;
                        title = "An unexpected error occurred.";
                        break;
                }
                // note : below is all the statuscode result of above exception
                //[ProducesResponseType(StatusCodes.Status200OK)]
                //[ProducesResponseType(StatusCodes.Status400BadRequest)]
                //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
                //[ProducesResponseType(StatusCodes.Status403Forbidden)]
                //[ProducesResponseType(StatusCodes.Status404NotFound)]
                //[ProducesResponseType(StatusCodes.Status408RequestTimeout)]
                //[ProducesResponseType(StatusCodes.Status409Conflict)]
                //[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
                //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
                // Use the dynamic methods to generate an error response
                return ProblemDetailHelper.CreateErrorResponse(
                    errorKey: errorKey,
                    title: title,
                    status: statusCode,
                    detail: exception.Message,
                    httpContext: httpContext
                );
            }
        }
        public class ErrorResponse
        {
            public string Type { get; set; } = "about:blank"; // A URI identifying the problem type
            public string Title { get; set; } = "An error occurred"; // Short summary
            public int Status { get; set; } // HTTP status code
            public dynamic? Detail { get; set; } // Detailed error message
            public string Instance { get; set; } = string.Empty; // URI to the instance

        }
    }
}
