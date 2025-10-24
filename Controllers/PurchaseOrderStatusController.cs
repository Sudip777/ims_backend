using inventory_management_system.Constants;
using inventory_management_system.Exceptions;
using inventory_management_system.Filters;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [ApiController]
    [Route(ApiRoutes.PurchaseOrderStatus.Base)]
    [Authorize]
    [RolePermission]
    public class PurchaseOrderStatusController: ControllerBase
    {
        private readonly IPurchaseOrderStatusService _purchaseOrderStatusService;
        private readonly ILogger<PurchaseOrderStatusController> _logger;
        public PurchaseOrderStatusController(IPurchaseOrderStatusService purchaseOrderStatusService, ILogger<PurchaseOrderStatusController> logger)
        {
          _purchaseOrderStatusService = purchaseOrderStatusService;
          _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPurchaseOrderStatuses()
        {

            try
            {

                var response = await _purchaseOrderStatusService.GetAllPurchaseOrderStatusesAsync();
                return Ok(new
                {
                    message = "Purchase Order Statuses Retrieved Successfully.",
                    result = response,
                    response_code = "00",

                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex,
                    "Invalid operation while retrieving purchase order status: by user {UserId}",
                    User.Identity?.Name ?? "Anonymous"
                  );

                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception while retrieving purchase order statuses:  by user {UserId}",
                    User.Identity?.Name ?? "Anonymous"
                    );

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }

        }
    }
}
