using static inventory_management_system.Helpers.ExceptionHandler;

namespace inventory_management_system.Helpers
{
    public class ProblemDetailHelper
    {
        public static string GetProblemDetailsType(string problemType)
        {
            DateTime now = DateTime.Now;


            string formattedDateTime = now.ToString("yyyyMMddHHmmss");
            return $"tag:ChaBaseCoreAPI,{formattedDateTime}:{problemType}";
        }

        


        public static string GetInstance(HttpContext httpContext)
        {
            // Provide a valid instance path or default value 
            return httpContext?.Request.Path.ToString() ?? "unknown-instance";
        }
        public static ErrorResponse CreateErrorResponse(string errorKey, string title, int status, string detail, HttpContext httpContext)
        {
            return new ErrorResponse
            {
                Type = GetProblemDetailsType(errorKey),
                Title = title,
                Status = status,
                Detail = detail,
                Instance = GetInstance(httpContext)
            };
        }
    }
}
