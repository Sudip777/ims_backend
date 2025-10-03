using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static ObjectResult ValidationProblem(
            this ControllerBase controller,
            ValidationResult validationResult)
        {
            return Helpers.ProblemDetailHelper.CreateValidationErrorObjectResult(
                validationResult,
                controller.HttpContext);
        }
    }
}