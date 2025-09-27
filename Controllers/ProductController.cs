using inventory_management_system.DTOs.Requests;
using inventory_management_system.Helpers;
using inventory_management_system.Models;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("getProductById/{id}")]
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
                _logger.LogError(ex, "Error occurred while creating a role.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }

        }

        [HttpGet("getAllProducts")]

        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
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
                _logger.LogError(ex, "Error occurred while creating a role.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpPost("createProduct")]

        public async Task<IActionResult> CreateProduct([FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }
            try
            {
                var createdProduct = await _productService.RegisterProductAsync(dto);
                return Ok(new
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
                _logger.LogError(ex, "Error occurred while creating a role.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }
            // Validate product business rules
            await _productService.ValidateProduct(dto, id);

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
                _logger.LogError(ex, "Error occurred while updating a product.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                {
                    return NotFound(new { Message = $"Product with ID {id} not found." });
                }
                return Ok(new
                {
                    message = "Product Deleted Successfully",
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a product.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }
    }
}