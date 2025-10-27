using inventory_management_system.Exceptions;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace inventory_management_system.Repositories
{
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
            var userRoleIdString = User?.FindFirst(ClaimTypes.Role)?.Value ?? "1";
            int userRoleId = int.TryParse(userRoleIdString, out var parsedRoleId) ? parsedRoleId : 1;

            var navItems = await _menuService.GetMenuItemsAsync(userRoleId);

            if (navItems == null || !navItems.Any()) //best practice
            {
                return NotFound();
            }

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
