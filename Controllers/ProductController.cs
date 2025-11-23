using FluentValidation;
using inventory_management_system.Constants;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Exceptions;
using inventory_management_system.Extensions;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace inventory_management_system.Controllers
{
    [Route(ApiRoutes.Products.Base)]
    [ApiController]
    [Authorize]
    [RolePermission]
    // <summary>
    // Manages product inventory operations
    // </summary>
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly IValidator<ProductDto> _validator;

        public ProductController(IProductService productService, ILogger<ProductController> logger, IValidator<ProductDto>  validator)
        {
            _productService = productService;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet(ApiRoutes.Products.ById)]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProductById(int id)
        {

            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { Message = $"Product with ID {id} not found." });
                }
                return Ok(new
                {
                    message = "Products Fetched Successfully",
                    result = product,
                    response_code = "00"
                });


            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
           $"Unhandled exception while retrieving product of {id}, by user {User.Identity?.Name}",id,
           User.Identity?.Name ?? "Anonymous"
           );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }

        }
        /// <summary>
        /// Retrieves a paginated list of products with filtering and sorting options
        /// </summary>
        /// <param name="request">Query parameters for filtering, sorting, and pagination</param>
        /// <returns>Paginated list of products</returns>
        /// <response code="200">Successfully retrieved products</response>
        /// <response code="401">Unauthorized - invalid or missing JWT token</response>
        /// <response code="404">No products found</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/products?page=1&amp;pageSize=10&amp;search=laptop&amp;categoryId=1&amp;sortColumn=name&amp;sortDirection=asc
        ///     
        /// Query Parameters:
        /// - **CategoryId**: Filter by category (optional)
        /// - **SupplierId**: Filter by supplier (optional)
        /// - **Search**: Search in product name or SKU (optional)
        /// - **SortColumn**: Column to sort by (name, price, stock, etc.)
        /// - **SortDirection**: Sort order (asc or desc)
        /// - **Page**: Page number (required, default: 1)
        /// - **PageSize**: Items per page (required, default: 10, max: 100)
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsRequest request)
        {
            try
            {
                var products = await _productService.GetAllProductsAsync(request);
                if (products.Data == null || !products.Data.Any())
                {
                    return NotFound(new
                    {
                        message = "No Products Found.",
                    });
                }
                return Ok(new
                {
                    message = "Products Fetched Successfully",
                    result = products,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
           $"Unhandled exception while retrieving products, by user {User.Identity?.Name}",
           User.Identity?.Name ?? "Anonymous"
           );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpGet("api/products/lists")]
        [ProducesResponseType(typeof(ProductDropdownResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProductsLists()
        {
            try
            {
                var products = await _productService.GetAllProductLists();
            return Ok(new
            {
                message = "Products Fetched Successfully",
                result = products,
                response_code = "00"
            });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
           $"Unhandled exception while retrieving products, by user {User.Identity?.Name}",
           User.Identity?.Name ?? "Anonymous"
           );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto dto)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for product creation: Errors: {@Errors}",
               result.Errors.Select(e => e.ErrorMessage));
                return this.ValidationProblem(result);

            }

            try
            {
                var createdProduct = await _productService.RegisterProductAsync(dto);
                return Ok( 
                     new
                     {
                         message = "Product Created Successfully",
                         result = createdProduct,
                         response_code = "00"
                     });

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
           $"Unhandled exception while creating product, by user {User.Identity?.Name}",
           User.Identity?.Name ?? "Anonymous"
           );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpPut(ApiRoutes.Products.ById)]
        [ProducesResponseType(typeof(InventoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto dto)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for product update: Errors: {@Errors}",
               result.Errors.Select(e => e.ErrorMessage));
                return this.ValidationProblem(result);

            }

            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(id, dto);
                return Ok(new
                {
                    message = "Product Updated Successfully",
                    result = updatedProduct,
                    response_code = "00"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
           $"Unhandled exception while updating product, by user {User.Identity?.Name}",
           User.Identity?.Name ?? "Anonymous"
           );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpDelete(ApiRoutes.Orders.ById)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
           $"Unhandled exception while deleting product, by user {User.Identity?.Name}",
           User.Identity?.Name ?? "Anonymous"
           );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }
    }
}