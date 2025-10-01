using inventory_management_system.Constants;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Exceptions;
using inventory_management_system.Services.Implementations;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace inventory_management_system.Controllers
{
    [Route(ApiRoutes.ProductSuppliers.Base)]
    [ApiController]
    [Authorize]
    public class ProductSupplierController:ControllerBase
    {
        private readonly IProductSupplierService _productSupplierService;
        private readonly ILogger<ProductSupplierController> _logger;
        public ProductSupplierController(IProductSupplierService productSupplierService)
        {
            _productSupplierService = productSupplierService;
        }
        [HttpGet]
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
                _logger.LogError(ex, "Error occurred while creating a role.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpGet(ApiRoutes.ProductSuppliers.ById)]
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
                _logger.LogError(ex, "Error occurred while creating a role.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateProductSupplier([FromBody] ProductSupplierDto dto)
        {
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
                _logger.LogError(ex, "Error occurred while creating a role.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }

    }
}
