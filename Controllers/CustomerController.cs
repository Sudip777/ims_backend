using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Exceptions;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace inventory_management_system.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController:ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _logger = logger;
            _customerService = customerService;
        }


        [HttpGet("getAllCustomers")]
        [ProducesResponseType(typeof(IEnumerable<CustomerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllCustomerAsync();
                return  Ok(new
                {
                    message = "Customers Retrieved Successfully",
                    result = customers,
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
                    ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpGet("getCustomerById/{id}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                return  Ok(new
                {
                    message = "Customer Retrieved Successfully",
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
                _logger.LogError(ex, "Error occurred while retrieving inventories.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpPost("createCustomer")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return new BadRequestObjectResult(new { message = "Validation failed", errors = errors });
            }
            try
            {
                var createdCustomer = await _customerService.CreateCustomerAsync(customerDto);
                return  Ok(new
                {
                    message = "Customer Created Successfully",
                    result = createdCustomer,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an inventory.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpPut("updateCustomer/{id}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return new BadRequestObjectResult(new { message = "Validation failed", errors = errors });
            }
            try
            {
                var updatedCustomer = await _customerService.UpdateCustomerAsync(id, customerDto);
                return  Ok(new
                {
                    message = "Customer Updated Successfully",
                    result = updatedCustomer,
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
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));

            }
        }

        [HttpDelete("deleteCustomer/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var result = await _customerService.DeleteCustomerAsync(id);
                if (!result)
                {
                    return new NotFoundObjectResult(new
                    {
                        message = $"Customer with ID {id} not found.",
                        response_code = "01"
                    });
                }
                return  Ok(new
                {
                    message = "Customer Deleted Successfully",
                   
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "User {Id} not found for deletion.", id);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operation failed for user {Id}: {Message}", id, ex.Message);
                return StatusCode((int)HttpStatusCode.Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting user {Id}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }
    }

}