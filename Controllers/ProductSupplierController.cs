using FluentValidation;
using inventory_management_system.Constants;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Exceptions;
using inventory_management_system.Extensions;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [Route(ApiRoutes.ProductSuppliers.Base)]
    [ApiController]
    [Authorize]
    public class ProductSupplierController:ControllerBase
    {
        private readonly IProductSupplierService _productSupplierService;
        private readonly ILogger<ProductSupplierController> _logger;
        private readonly IValidator<ProductSupplierDto> _validator;

        public ProductSupplierController(IProductSupplierService productSupplierService, IValidator<ProductSupplierDto> validator
            )
        {
            _productSupplierService = productSupplierService;
            _validator = validator;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ProductSupplierResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllProductSuppliers()
        {
            try
            {
                var data = await _productSupplierService.GetAllProductSuppliersAsync();
               
                return Ok(new
                {
                    message = "Product Suppliers Retrieved Successfully",
                    result = data,
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
           $"Unhandled exception while retrieving product suppliers, by user {User.Identity?.Name}",
           User.Identity?.Name ?? "Anonymous"
           );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpGet(ApiRoutes.ProductSuppliers.ById)]
        [ProducesResponseType(typeof(ProductSupplierResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductSupplierById(int id)
        {
            try
            {
                var res = await _productSupplierService.GetProductSupplierByIdAsync(id);
                return Ok(new
                {
                    message = "Product Supplier Retrieved Successfully",
                    result = res,
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
           $"Unhandled exception while retrieving product supplier of {id}, by user {User.Identity?.Name}",id,
           User.Identity?.Name ?? "Anonymous"
           );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(ProductSupplierDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateProductSupplier([FromBody] ProductSupplierDto dto)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for product supplier creation: Errors: {@Errors}",
               result.Errors.Select(e => e.ErrorMessage));
                return this.ValidationProblem(result);

            }

            try
            {
                var res = await _productSupplierService.CreateProductSupplierAsync(dto);
                return Ok(new
                {
                    message = "Product Supplier Created Successfully",
                    result = res,
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
           $"Unhandled exception while creating product supplier, by user {User.Identity?.Name}",
           User.Identity?.Name ?? "Anonymous"
           );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

    }
}
