using Azure;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Exceptions;
using inventory_management_system.Services.Implementations;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace inventory_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]

    public class RoleController:ControllerBase
       
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;

        }

        [HttpPost("createRoles")]
        [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto dto)
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
                _logger.LogError(ex, "Error occurred while creating a role.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpPost("getRoles")]
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
                _logger.LogError(ex, "Error occurred while creating a role.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }

    }
}
