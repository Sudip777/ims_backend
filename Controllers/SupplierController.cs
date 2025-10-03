using FluentValidation;
using inventory_management_system.Constants;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Exceptions;
using inventory_management_system.Extensions;
using inventory_management_system.Services.Implementations;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace inventory_management_system.Controllers
{

    [Route(ApiRoutes.Suppliers.Base)]
    [ApiController]
    [Authorize]
    public class SupplierController:ControllerBase
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly ISupplierService _supplierService;
        private readonly IValidator<SupplierDto> _validator;


        public SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger, IValidator<SupplierDto> validator)
        {
            _logger = logger;
            _supplierService = supplierService;
            _validator = validator;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SupplierResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllSuppliers()
        {
            try
            {
                var suppliers = await _supplierService.GetAllSupplierAsync();
                return Ok(new
                {
                    message = "Suppliers Retrieved Successfully",
                    result = suppliers,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving inventories.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpGet(ApiRoutes.Suppliers.ById)]
        [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            try
            {
                var customer = await _supplierService.GetSupplierByIdAsync(id);
                return Ok(new
                {
                    message = "Supplier Retrieved Successfully",
                    result = customer,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving supplier data.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierDto supplierDto)
        {
            var result = await _validator.ValidateAsync(supplierDto);
            if (!result.IsValid)
            {
                return this.ValidationProblem(result);

            }

            try
            {
                var createdSupplier = await _supplierService.CreateSupplierAsync(supplierDto);
                return Ok(new
                {
                    message = "Supplier Created Successfully",
                    result = createdSupplier,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a supplier.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpPut(ApiRoutes.Suppliers.ById)]
        [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierDto supplierDto)
        {
            var result = await _validator.ValidateAsync(supplierDto);
            if (!result.IsValid)
            {
                return this.ValidationProblem(result);

            }
            try
            {
                var updatedSupplier = await _supplierService.UpdateSupplierAsync(id, supplierDto);
                return Ok(new
                {
                    message = "Supplier Updated Successfully",
                    result = updatedSupplier,
                    response_code = "00"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating an inventory.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));

            }
        }

        [HttpDelete(ApiRoutes.Suppliers.ById)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]

        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                var result = await _supplierService.DeleteSupplierAsync(id);
                if (!result)
                {
                    return new NotFoundObjectResult(new
                    {
                        message = $"Supplier with ID {id} not found.",
                        
                    });
                }
                return Ok(new
                {
                    message = "Supplier Deleted Successfully",

                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Supplier {Id} not found for deletion.", id);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operation failed for supplier {Id}: {Message}", id, ex.Message);
                return StatusCode((int)HttpStatusCode.Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting supplier {Id}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
