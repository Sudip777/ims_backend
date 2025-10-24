using inventory_management_system.Constants;
using inventory_management_system.Exceptions;
using inventory_management_system.Filters;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Dashboard.Base)]
    [RolePermission]
    public class DashboardController:ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardService _dashboardService;
        public DashboardController(ILogger<DashboardController> logger,IDashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }
        [HttpGet(ApiRoutes.Dashboard.InventoryOverview)]
        public async Task<IActionResult> GetInventoryOverview()
        {
            try
            {
                var response = await _dashboardService.GetInventoryOverview();
               
                return Ok(new
                {
                    message = "Inventory Overview Data Retrieved Successfully",
                    result = response,
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
                $"Unhandled exception while retriving inventory overview, by user {User.Identity?.Name}",
                User.Identity?.Name ?? "Anonymous"
                );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpGet(ApiRoutes.Dashboard.Sales)]
        public async Task<IActionResult> GetSalesPerformance()
        {
            try
            {
                var response = await _dashboardService.GetSalesPerformance();
                return Ok(new
                {
                    message = "Sales Performance Data Retrieved Successfully",
                    result = response,
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
                $"Unhandled exception while retrieving sales performance dataa, by user {User.Identity?.Name}",
                User.Identity?.Name ?? "Anonymous"
                );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpGet(ApiRoutes.Dashboard.Summary)]
        public async Task<IActionResult> GetPurchseOrderStatus()
        {
            try
            {
                var response = await _dashboardService.GetPurchaseOrderStatuses();
                return Ok(new
                {
                    message = "Purchase Order Status Data Retrieved Successfully",
                    result = response,
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
                $"Unhandled exception while retrieving sales purchase data, by user {User.Identity?.Name}",
                User.Identity?.Name ?? "Anonymous"
                );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }
        [HttpGet(ApiRoutes.Dashboard.InventoryTransactinOverview)]
        public async Task<IActionResult> GetInventoryTransactionOverview()
        {
            try
            {
                var response = await _dashboardService.GetInventoryTransactionSummary();
                return Ok(new
                {
                    message = "Inventory Transaction Summary Retrieved Successfully",
                    result = response,
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
                $"Unhandled exception while retrieving invenotry transaction overview data, by user {User.Identity?.Name}",
                User.Identity?.Name ?? "Anonymous"
                );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }
        [HttpGet(ApiRoutes.Dashboard.Warehouse)]
        public async Task<IActionResult> GetWarehouseOverview()
        {
            try
            {
                var response = await _dashboardService.GetWarehouseOverview();
                return Ok(new
                {
                    message = "Warehouse Overview Data Retrieved Successfully",
                    result = response,
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
                $"Unhandled exception while retrieving warehouse overview data, by user {User.Identity?.Name}",
                User.Identity?.Name ?? "Anonymous"
                );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }
    }
}
