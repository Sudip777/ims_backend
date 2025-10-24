using FluentValidation;
using inventory_management_system.Constants;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Exceptions;
using inventory_management_system.Extensions;
using inventory_management_system.Filters;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Categories.Base)]
    [Authorize]
    [RolePermission]
    public class CategoryController:ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        private readonly IValidator<CategoryDto> _validator;


        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger, IValidator<CategoryDto> validator)
        {
           _categoryService = categoryService;
              _logger = logger;
            _validator = validator;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var response = await _categoryService.GetAllCategoriesAsync();
                _logger.LogInformation(
                   $"Category Retrieved successfully by user {User.Identity?.Name}", User.Identity?.Name ?? "Anonymous"
                  );
                return Ok(new
                {
                    message = "Categories Retrieved Successfully",
                    result = response,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex,
                  $"Invalid operation while retrieving categories: by user {User.Identity?.Name} ", User.Identity?.Name ?? "Anonymous"
                  );
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Unhandled exception while retrieving categories: by user {User.Identity?.Name}",
                    User.Identity?.Name ?? "Anonymous");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpGet(ApiRoutes.Categories.ById)]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById( int categoryId)
        {
            try
            {
                var response = await _categoryService.GetCategoryByIdAsync(categoryId);
                _logger.LogInformation(
                  $"Category of {categoryId} Retrieved successfully by user {User.Identity?.Name}",categoryId,  User.Identity?.Name ?? "Anonymous"
                 );
                return Ok(new
                {
                    message = "Category Retrieved Successfully",
                    result = response,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex,
                    $"Invalid operation while retrieving category of {categoryId} by user {User.Identity?.Name}",categoryId, User.Identity?.Name ?? "Anonymous",
                    HttpContext.TraceIdentifier);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception while retrieving category of {categoryId}: by user {User.Identity?.Name}",categoryId,
                    User.Identity?.Name ?? "Anonymous"
                    );
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
           
            var validationResult = await _validator.ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for category creation: {CategoryName}. Errors: {@Errors}",
                    categoryDto.CategoryName,
                    validationResult.Errors.Select(e => e.ErrorMessage));

                return this.ValidationProblem(validationResult);
            }

            try
            {
                var response = await _categoryService.CreateCategoryAsync(categoryDto);

                _logger.LogInformation(
                    "Category created successfully: {CategoryName}, by user {UserId}",
                    response.CategoryName,
                    User.Identity?.Name ?? "Anonymous"
                    );

                return CreatedAtAction(
                    nameof(GetCategoryById),   
                    new
                    {
                        message = "Category Created Successfully",
                        result = response,
                        response_code = "00"
                    });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex,
                    "Invalid operation while creating category: {CategoryName}, by user {UserId}",
                    categoryDto.CategoryName,
                    User.Identity?.Name ?? "Anonymous"
                  );

                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception while creating category: {CategoryName}, by user {UserId}",
                    categoryDto.CategoryName,
                    User.Identity?.Name ?? "Anonymous"
                    );

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

    }
}
