using Azure;
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
    [ApiController]
    [Route(ApiRoutes.UrlEndpoints.Base)]
    [Authorize(Roles = RoleConstants.SUPER_ADMIN)]
    public class UrlEndpointController:ControllerBase
    {
        private readonly IUrlEndpointService _urlEndpointService;
        private readonly ILogger<SupplierController> _logger;
        private readonly IValidator<UrlEndpointDto> _validator;


        public UrlEndpointController(IUrlEndpointService urlEndpointService, ILogger<SupplierController> logger, IValidator<UrlEndpointDto> validator)
        {
            _urlEndpointService = urlEndpointService;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllUrlEndpoints()
        {
            try
            {
                var data = await _urlEndpointService.GetAllUrlEndpointsAsync();
                return Ok(new
                {
                    message = "UrlEndpoints Retrieeved Successfully.",
                    result = data,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex,
                  $"Invalid operation while retrieving url endpoints: by user {User.Identity?.Name} ", User.Identity?.Name ?? "Anonymous"
                  );
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                 $"Unhandled exception while retrieving url endpoints, by user {User.Identity?.Name}",
                 User.Identity?.Name ?? "Anonymous"
                 );
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }
        
        [HttpGet(ApiRoutes.UrlEndpoints.ById)]
        [ProducesResponseType(typeof(UrlEndpointResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUrlEndpointById(int id)
        {
            try
            {
                var data = await _urlEndpointService.GetUrlEndpointByIdAsync(id);
                return Ok(new
                {
                    message = "UrlEndpoint Retrieved Successfully.",
                    result = data,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex,
                  $"Invalid operation while retrieving url endpoint of {id}: by user {User.Identity?.Name} ",id, User.Identity?.Name ?? "Anonymous"
                  );
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                 $"Unhandled exception while retrieving url endpoint of {id}: by user {User.Identity?.Name}",id,
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
        public async Task<IActionResult> CreateUrlEndpoint(UrlEndpointDto dto)
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
                var data = await _urlEndpointService.CreateUrlEndpointAsync(dto);
                return Ok(
                  new
                    {
                        message = "Url Endpoint Created Successfully",
                        result = data,
                        response_code = "00"
                    });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex,
                    $"Invalid operation while creating url endpoint: {dto.Url}, by user { User.Identity?.Name}",
                    dto.Url,
                    User.Identity?.Name ?? "Anonymous"
                  );

                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Unhandled exception while creating Url endpoint: {dto.Url}, by user {User.Identity?.Name}",
                    dto.Url,
                    User.Identity?.Name ?? "Anonymous"
                    );

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpPut(ApiRoutes.UrlEndpoints.ById)]
        [ProducesResponseType(typeof(UrlEndpointDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUrlEndpoint(UrlEndpointDto dto, int id)
        {
            try
            {
                var data = await _urlEndpointService.UpdateUrlEndpointAsync(dto, id);
                return Ok(new
                {
                    message = "UrlEndpoint Updated Successfully.",
                    result = data,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {

                _logger.LogWarning(ex,
                    $"Invalid operation while updating url endpoint: {dto.Url} of :{id} by user {User.Identity?.Name}",
                    dto.Url,
                    id,
                    User.Identity?.Name ?? "Anonymous"
                  );
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                  $"Unhandled exception while updating endpoint url: {dto.Url}, by user {User.Identity?.Name}",
                  dto.Url,
                  User.Identity?.Name ?? "Anonymous"
                  );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));

            }
        }

        [HttpDelete(ApiRoutes.UrlEndpoints.ById)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> DeleteEndpoint(int id)
        {
            try
            {
                var deleted = await _urlEndpointService.DeleteUrlEndpointAsync(id);

                if (!deleted)
                    return NotFound(new { message = $"Url Endpoint with ID {id} not found." });

                return NoContent(); 
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operation failed for Url Endpoint {Id}: {Message}", id, ex.Message);
                return StatusCode((int)HttpStatusCode.Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception while deleting Url Endpoint {Id} by user {User}",
                    id, User.Identity?.Name ?? "Anonymous"
                );
                return StatusCode((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }


    }
}
