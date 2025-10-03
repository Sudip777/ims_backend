using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace inventory_management_system.Helpers
{
    /// <summary>
    /// Helper class for creating RFC 7807 compliant problem details responses.
    /// </summary>
    public static class ProblemDetailHelper
    {
        // Define sensitive fields that should never expose their values
        private static readonly HashSet<string> SensitiveFields = new(StringComparer.OrdinalIgnoreCase)
        {
            "Password", "ConfirmPassword", "OldPassword", "NewPassword",
            "Pin", "Token", "Secret", "ApiKey", "RefreshToken", "AccessToken",
            "CreditCard", "Ssn", "SecurityAnswer"
        };

        /// <summary>
        /// Gets a static problem details type URL for consistent error identification.
        /// </summary>
        /// <param name="problemType">The type of problem (e.g., 'validation-error').</param>
        /// <returns>A URI reference for the problem type.</returns>
        public static string GetProblemDetailsType(string problemType)
        {
            return $"https://api.inventorymanagementsystem.com/problems/{problemType}";
        }

        /// <summary>
        /// Gets the request instance path.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>The request path or '/unknown' if not available.</returns>
        public static string GetInstance(HttpContext httpContext)
        {
            return httpContext?.Request.Path.ToString() ?? "/unknown";
        }

        /// <summary>
        /// Creates a standard error response for general errors.
        /// </summary>
        /// <param name="errorKey">The key identifying the error type.</param>
        /// <param name="title">A short, human-readable summary of the error.</param>
        /// <param name="status">The HTTP status code.</param>
        /// <param name="detail">A human-readable explanation of the error.</param>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>An RFC 7807 compliant error response.</returns>
        public static ErrorResponse CreateErrorResponse(
            string errorKey,
            string title,
            int status,
            string detail,
            HttpContext httpContext)
        {
            return new ErrorResponse
            {
                Type = GetProblemDetailsType(errorKey),
                Title = title,
                Status = status,
                Detail = detail,
                Instance = GetInstance(httpContext),
                TraceId = httpContext?.TraceIdentifier
            };
        }

        /// <summary>
        /// Creates a validation error response from FluentValidation results.
        /// </summary>
        /// <param name="validationResult">The FluentValidation result containing errors.</param>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>An RFC 7807 compliant validation error response with grouped errors.</returns>
        public static ErrorResponse CreateValidationErrorResponse(
            ValidationResult validationResult,
            HttpContext httpContext)
        {
            var errorsByField = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => ToCamelCase(g.Key),
                    g => g.Select(e => new ValidationError
                    {
                        Code = e.ErrorCode,
                        Message = e.ErrorMessage
                    })
                    .DistinctBy(e => e.Code) // Deduplicate by error code
                    .ToArray()
                );

            return new ErrorResponse
            {
                Type = GetProblemDetailsType("validation-error"),
                Title = "One or more validation errors occurred.",
                Status = 400,
                Detail = "The request contains invalid data. Please review the errors and try again.",
                Instance = GetInstance(httpContext),
                Errors = errorsByField,
                TraceId = httpContext?.TraceIdentifier
            };
        }

        /// <summary>
        /// Creates a validation error response as ObjectResult.
        /// </summary>
        /// <param name="validationResult">The FluentValidation result containing errors.</param>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>An ObjectResult with an RFC 7807 compliant validation error response.</returns>
        public static ObjectResult CreateValidationErrorObjectResult(
            ValidationResult validationResult,
            HttpContext httpContext)
        {
            var errorResponse = CreateValidationErrorResponse(validationResult, httpContext);
            return new ObjectResult(errorResponse)
            {
                StatusCode = 400,
                ContentTypes = { "application/problem+json" }
            };
        }

        /// <summary>
        /// Formats a single validation error message, protecting sensitive data.
        /// </summary>
        /// <param name="failure">The FluentValidation failure.</param>
        /// <returns>A formatted error message and code.</returns>
        private static ValidationError FormatErrorMessage(ValidationFailure failure)
        {
            return new ValidationError
            {
                Code = failure.ErrorCode,
                Message = failure.ErrorMessage
            };
        }

        /// <summary>
        /// Checks if a field is sensitive and should not expose its value.
        /// </summary>
        /// <param name="propertyName">The name of the property to check.</param>
        /// <returns>True if the field is sensitive, false otherwise.</returns>
        private static bool IsSensitiveField(string propertyName)
        {
            return SensitiveFields.Contains(propertyName) ||
                   propertyName.Contains("Password", StringComparison.OrdinalIgnoreCase) ||
                   propertyName.Contains("Secret", StringComparison.OrdinalIgnoreCase) ||
                   propertyName.Contains("Token", StringComparison.OrdinalIgnoreCase) ||
                   propertyName.Contains("Key", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if a type is a simple value type safe to display.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is safe to display, false otherwise.</returns>
        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive ||
                   type.IsEnum ||
                   type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }

        /// <summary>
        /// Converts property name to camelCase for JSON consistency.
        /// </summary>
        /// <param name="str">The property name to convert.</param>
        /// <returns>The camelCase version of the property name.</returns>
        private static string ToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
                return str;
            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }
    }

    /// <summary>
    /// Represents a single validation error with a code and message.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// The machine-readable error code.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The human-readable error message.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// RFC 7807 compliant error response model.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// A URI reference that identifies the problem type.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// A short, human-readable summary of the problem type.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The HTTP status code.
        /// </summary>
        [JsonPropertyName("status")]
        public int Status { get; set; }

        /// <summary>
        /// A human-readable explanation specific to this occurrence.
        /// </summary>
        [JsonPropertyName("detail")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Detail { get; set; }

        /// <summary>
        /// A URI reference that identifies the specific occurrence.
        /// </summary>
        [JsonPropertyName("instance")]
        public string Instance { get; set; } = string.Empty;

        /// <summary>
        /// Validation errors grouped by field name.
        /// </summary>
        [JsonPropertyName("errors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, ValidationError[]>? Errors { get; set; }

        /// <summary>
        /// Trace identifier for debugging and log correlation.
        /// </summary>
        [JsonPropertyName("traceId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TraceId { get; set; }
    }
}