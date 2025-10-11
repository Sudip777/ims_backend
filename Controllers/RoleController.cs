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


namespace inventory_management_system.Controllers
{
    [Route(ApiRoutes.Roles.Base)]
    [ApiController]
    [Authorize(Roles = "ADMIN")]

    public class RoleController:ControllerBase
       
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;
        private readonly IValidator<RoleDto> _validator;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger, IValidator<RoleDto> validator)
        {
            _roleService = roleService;
            _logger = logger;
            _validator = validator;

        }

        [HttpGet]
        [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(new
                {
                    message = "Role Fetched Successfully",
                    result = roles,
                    response_code = "00"
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Roles Not Found");
                return NotFound(ex.Message);
            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
           $"Unhandled exception while retrieving  roles, by user {User.Identity?.Name}",
           User.Identity?.Name ?? "Anonymous"
           );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto dto)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for role creation: Errors: {@Errors}",
               result.Errors.Select(e => e.ErrorMessage));
                return this.ValidationProblem(result);

            }

            try
            {
                
                var role = dto.MappedRole();
                var createdRole = await _roleService.RegisterRoleAsync(role);

                if(createdRole == null)
                {
                    throw new KeyNotFoundException($"Error while Creating Role");

                }

                return Ok(new
                {
                    message = "Role Created Successfully",
                    result = createdRole,
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
                   $"Unhandled exception while creating role {dto.RoleName}, by user {User.Identity?.Name}",dto.RoleName,
                   User.Identity?.Name ?? "Anonymous"
                   );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

    }
}
