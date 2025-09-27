namespace inventory_management_system.Constants
{
  
        public static class ErrorConstants
        {
            // Error Keys
            public const string BAD_REQUEST = "BadRequest";
            public const string UNAUTHORIZED = "Unauthorized";
            public const string FORBIDDEN = "Forbidden";
            public const string NOT_FOUND = "NotFound";
            public const string CONFLICT = "Conflict";
            public const string DATABASE_ERROR = "DatabaseError";
            public const string TIMEOUT = "Timeout";
            public const string UNPROCESSABLE_ENTITY = "UnprocessableEntity";
            public const string INTERNAL_SERVER_ERROR = "InternalServerError";

            // Error Titles
            public static class Titles
            {
                public const string INVALID_INPUT = "Invalid input parameters.";
                public const string USER_NOT_AUTHORIZED = "User is not authorized.";
                public const string ACCESS_FORBIDDEN = "Access to this resource is forbidden.";
                public const string RESOURCE_NOT_FOUND = "The requested resource was not found.";
                public const string REQUEST_CONFLICT = "The request conflicts with the current state.";
                public const string DATABASE_ERROR = "A database error occurred.";
                public const string REQUEST_TIMEOUT = "The request timed out while processing.";
                public const string BUSINESS_RULE_FAILED = "Business rule validation failed.";
                public const string UNEXPECTED_ERROR = "An unexpected error occurred.";
            }

            // Problem Detail Types
            public static class Types
            {
                public const string ABOUT_BLANK = "about:blank";
                public const string VALIDATION_ERROR = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                public const string AUTHENTICATION_ERROR = "https://tools.ietf.org/html/rfc7235#section-3.1";
            }
        }
    
}
