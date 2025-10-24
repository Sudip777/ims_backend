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
using System.Net;

namespace inventory_management_system.Controllers
{

    [Route(ApiRoutes.Customers.Base)]
    [ApiController]
    [Authorize]
    [RolePermission]
    public class CustomerController:ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        private readonly IValidator<CustomerDto> _validator;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger, IValidator<CustomerDto> validator)
        {
            _logger = logger;
            _customerService = customerService;
            _validator = validator;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                _logger.LogError(ex,
                $"Unhandled exception while retrieving customers, by user {User.Identity?.Name}",
                User.Identity?.Name ?? "Anonymous"
                );
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpGet(ApiRoutes.Customers.ById)]
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
                _logger.LogError(ex,
                $"Unhandled exception while retrieving customer with {id} by user {User.Identity?.Name}",id,
                User.Identity?.Name ?? "Anonymous"
                );
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            var result = await _validator.ValidateAsync(customerDto);
            if (!result.IsValid)
            {
                _logger.LogWarning($"Validation failed for customer creation: {customerDto.Name}. Errors: {result.Errors.Select(e => e.ErrorMessage)}",
                   customerDto.Name,
                   result.Errors.Select(e => e.ErrorMessage));
                return this.ValidationProblem(result);

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
                _logger.LogWarning(ex,
                   $"Invalid operation while creating customer: {customerDto.Name}, by user {User.Identity?.Name ?? "Anonymous"}",
                   customerDto.Name,
                   User.Identity?.Name ?? "Anonymous"
                 );
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                   $"Unhandled exception while creating customer: {customerDto.Name}, by user {User.Identity?.Name}",
                   customerDto.Name,
                   User.Identity?.Name ?? "Anonymous"
                   );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpPut(ApiRoutes.Customers.ById)]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
        {
            var result = await _validator.ValidateAsync(customerDto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for customer update: {Name}. Errors: {@Errors}",
                  customerDto.Name,
                  result.Errors.Select(e => e.ErrorMessage));
                return this.ValidationProblem(result);

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
                _logger.LogError(ex,
                  $"Unhandled exception while updating customer: {customerDto.Name}, by user {User.Identity?.Name}",
                  customerDto.Name,
                  User.Identity?.Name ?? "Anonymous"
                  );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));

            }
        }

        [HttpDelete(ApiRoutes.Customers.ById)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
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
                return NoContent();
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
                _logger.LogError(ex,
                 $"Unhandled exception while deleting customer, by user {User.Identity?.Name}",
                 User.Identity?.Name ?? "Anonymous"
                 );
                return StatusCode((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }
    }

}