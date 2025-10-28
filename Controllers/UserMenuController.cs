using inventory_management_system.Constants;
using inventory_management_system.Exceptions;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Repositories
{
    [ApiController]
    [Route(ApiRoutes.UserMenu.Base)]
    [Authorize]
    public class UserMenuController : ControllerBase
    {
        private readonly IUserMenuService _menuService;
        private readonly ILogger<UserMenuController> _logger;

        public UserMenuController(IUserMenuService menuService, ILogger<UserMenuController> logger)
        {
            _menuService = menuService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenuEndpoints()
        {
            try
            {
                var roleIdClaim = User?.FindFirst("RoleId")?.Value;
                if (!int.TryParse(roleIdClaim, out var roleId) || roleId <= 0)
                    return Unauthorized("Invalid role.");

                var navItems = await _menuService.GetMenuItemsAsync(roleId);
                if (navItems == null || !navItems.Any())
                    return NotFound(new { Message = "No menu items found.", response_code = "01" });

                return Ok(new
                {
                    Message = "Menu Items Fetched Successfully.",
                    result = navItems,
                    response_code = "00"
                });

            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(new { message = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,
                  $"Unhandled exception while retrieving warehouses, by user {User.Identity?.Name}",
                  User.Identity?.Name ?? "Anonymous"
                  );
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(exception, HttpContext));
            }


        }
    }
}
