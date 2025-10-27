using FluentValidation;
using inventory_management_system.Constants;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Enums;
using inventory_management_system.Exceptions;
using inventory_management_system.Extensions;
using inventory_management_system.Filters;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace inventory_management_system.Controllers
{
    [ApiController]
    [Route(ApiRoutes.RolePermissions.Base)]
    [Authorize]
    [RolePermission]

    public class RolePermissionController:ControllerBase
    {
        private readonly IRolePermissionService _rolePermissionService;
        private readonly ILogger<RolePermissionController> _logger;
        private readonly IValidator<RolePermissionDto> _validator;

        public RolePermissionController(IRolePermissionService rolePermissionService, ILogger<RolePermissionController> logger, IValidator<RolePermissionDto> validator)
        {
            _rolePermissionService = rolePermissionService;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(RolePermissionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllRolePermissions()
        {
            try
            {
                var data = await _rolePermissionService.GetAllRolePermissionsAsync();
                return Ok(new
                {
                    message = "Role Permissions Retrieved Successfully.",
                    result = data,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex,
                  $"Invalid operation while retrieving role permissions: by user {User.Identity?.Name} ", User.Identity?.Name ?? "Anonymous"
                  );
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                 $"Unhandled exception while retrieving role permissions, by user {User.Identity?.Name}",
                 User.Identity?.Name ?? "Anonymous"
                 );
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpGet(ApiRoutes.RolePermissions.ById)]
        [ProducesResponseType(typeof(UrlEndpointResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUrlEndpointById(int id)
        {
            try
            {
                var data = await _rolePermissionService.GetRolePermissionByIdAsync(id);
                return Ok(new
                {
                    message = "Role Permission Retrieved Successfully.",
                    result = data,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex,
                  $"Invalid operation while retrieving role permission of {id}: by user {User.Identity?.Name} ", id, User.Identity?.Name ?? "Anonymous"
                  );
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                 $"Unhandled exception while retrieving role permision of {id}: by user {User.Identity?.Name}", id,
                 User.Identity?.Name ?? "Anonymous"
                 );
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(RolePermissionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUrlEndpoint(RolePermissionDto dto)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for role permission creation: Errors: {@Errors}",
               result.Errors.Select(e => e.ErrorMessage));
                return this.ValidationProblem(result);

            }
            try
            {
                var data = await _rolePermissionService.CreateRolePermissionAsync(dto);
                return Ok(
                    new
                    {
                        message = "Role Permission Created Successfully",
                        result = data,
                        response_code = "00"
                    });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex,
                    $"Invalid operation while creating role permission, by user {User.Identity?.Name}",
                    User.Identity?.Name ?? "Anonymous"
                  );

                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Unhandled exception while creating role permissions, by user {User.Identity?.Name}",
                    User.Identity?.Name ?? "Anonymous"
                    );

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpPut(ApiRoutes.RolePermissions.ById)]
        [ProducesResponseType(typeof(RolePermissionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUrlEndpoint(RolePermissionDto dto, int id)
        {
            try
            {
                var data = await _rolePermissionService.UpdateRolePermissionAsync(dto, id);
                return Ok(new
                {
                    message = "Role Permission Updated Successfully.",
                    result = data,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex,
                    $"Invalid operation while updating role permission of :{id} by user {User.Identity?.Name}",
                    id,
                    User.Identity?.Name ?? "Anonymous"
                  );
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                  $"Unhandled exception while updating role permission, by user {User.Identity?.Name}",
                  User.Identity?.Name ?? "Anonymous"
                  );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpDelete(ApiRoutes.RolePermissions.ById)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> DeleteEndpoint(int id)
        {
            try
            {
                var deleted = await _rolePermissionService.DeleteRolePermissionAsync(id);

                if (!deleted)
                    return NotFound(new { message = $"Role Permission with ID {id} not found." });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operation failed for Role Permission {Id}: {Message}", id, ex.Message);
                return StatusCode((int)HttpStatusCode.Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception while deleting Role Permission {Id} by user {User}",
                    id, User.Identity?.Name ?? "Anonymous"
                );
                return StatusCode((int) HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

    }
}
