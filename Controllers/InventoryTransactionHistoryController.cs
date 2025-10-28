using inventory_management_system.Constants;
using inventory_management_system.Exceptions;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [ApiController]
    [Route(ApiRoutes.InventoryTransactionHistory.Base)]
    [Authorize]
    [RolePermission]
    public class InventoryTransactionHistoryController:ControllerBase
    {
        private readonly ILogger<InventoryTransactionHistoryController> _logger;
        private readonly IInventoryTransactionHistoryService _service;

       public InventoryTransactionHistoryController(ILogger<InventoryTransactionHistoryController> logger, IInventoryTransactionHistoryService service)
        {
            _logger = logger;
            _service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllInventoryTransactionHistories()
        {
            try
            {
                var response = await _service.GetAllInventoryTransactionHistoriesAsync();
                return Ok(new
                {
                    message = "Inventory transaction histories Retrieved Successfully.",
                    result = response,
                    response_code = "00",
                });
            }
          
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
           $"Unhandled exception while retrieving inventories history, by user {User.Identity?.Name}",
           User.Identity?.Name ?? "Anonymous"
           );
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }
    }
}
