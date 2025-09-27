using inventory_management_system.Constants;
using inventory_management_system.Helpers;
using Microsoft.Data.SqlClient;
using System.Security;
using static inventory_management_system.Constants.ErrorConstants;

namespace inventory_management_system.Exceptions
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
                        errorKey = ErrorConstants.BAD_REQUEST;
                        statusCode = StatusCodes.Status400BadRequest;
                        title = Titles.INVALID_INPUT;
                        break;

                    case UnauthorizedAccessException:
                        errorKey = ErrorConstants.UNAUTHORIZED;
                        statusCode = StatusCodes.Status401Unauthorized;
                        title = Titles.USER_NOT_AUTHORIZED;
                        break;

                    case SecurityException:
                        errorKey = ErrorConstants.FORBIDDEN;
                        statusCode = StatusCodes.Status403Forbidden;
                        title = Titles.ACCESS_FORBIDDEN;
                        break;

                    case KeyNotFoundException:
                        errorKey = ErrorConstants.NOT_FOUND;
                        statusCode = StatusCodes.Status404NotFound;
                        title = Titles.RESOURCE_NOT_FOUND;
                        break;

                    case InvalidOperationException:
                        errorKey = ErrorConstants.CONFLICT;
                        statusCode = StatusCodes.Status409Conflict;
                        title = Titles.REQUEST_CONFLICT;
                        break;
                    case SqlException:
                        errorKey = ErrorConstants.DATABASE_ERROR;
                        statusCode = StatusCodes.Status500InternalServerError;
                        title = Titles.DATABASE_ERROR;
                        break;

                    case TimeoutException:
                        errorKey = ErrorConstants.TIMEOUT;
                        statusCode = StatusCodes.Status408RequestTimeout;
                        title = Titles.REQUEST_TIMEOUT;
                        break;
                    case ApplicationException:
                        errorKey = ErrorConstants.UNPROCESSABLE_ENTITY;
                        statusCode = StatusCodes.Status422UnprocessableEntity;
                        title = Titles.BUSINESS_RULE_FAILED;
                        break;

                    default:
                        errorKey = ErrorConstants.INTERNAL_SERVER_ERROR;
                        statusCode = StatusCodes.Status500InternalServerError;
                        title = Titles.UNEXPECTED_ERROR;

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
