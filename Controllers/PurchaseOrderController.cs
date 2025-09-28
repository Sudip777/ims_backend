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
    [Authorize]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ILogger<PurchaseOrderController> _logger;

        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService, ILogger<PurchaseOrderController> logger)
        {
            _purchaseOrderService = purchaseOrderService;
            _logger = logger;

        }

        [HttpGet("getAllPurchaseOrders")]
        [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllPurchaseOrders()
        {
            var response = await _purchaseOrderService.GetAllPurchaseOrdersAsync();

            if (response == null || !response.Any())
            {
                return NotFound(Array.Empty<object>());
            }

            return Ok(new
            {
                message = "Purchase Orders Retrieved Successfully",
                result = response,
                response_code = "00"
            });
        }

        [HttpGet("getPurchaseOrderById/{id}")]
        [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllPurchaseOrders(int id)
        {

            try
            {
                var response = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);

                if (response == null)
                {
                    return NotFound(Array.Empty<object>());
                }

                return Ok(new
                {
                    message = "Purchase Orders Retrieved Successfully",
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
                _logger.LogError(ex, "Error occurred while retrieving inventories.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }

        }

        [HttpPost("createPurchaseOrder")]
        [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreatePurchaseOrders([FromBody] PurchaseOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Validation failed", errors = errors });
            }

            try
            {
                var createdPurchaseOrder = await _purchaseOrderService.CreatePurchaseOrderAsync(dto);

                if (createdPurchaseOrder == null)
                {
                    return Ok(Array.Empty<object>());
                }

                return Ok(new
                {
                    message = "Inventory Created Successfully",
                    result = createdPurchaseOrder,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an inventory.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpPut("updatePurchaseOrder/{id}")]
        [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdatePurchaseOrder([FromBody] PurchaseOrderDto dto, int id)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Validation failed", errors = errors });
            }
            try
            {
                var updatedOder = await _purchaseOrderService.UpdatePurchaseOrderAsync(dto, id);
                if (updatedOder == null)
                {
                    return NotFound(Array.Empty<object>());
                }
                return Ok(new
                {
                    message = "Purchase Order Updated Successfully",
                    result = updatedOder,
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
                _logger.LogError(ex, "Error occurred while updating an inventory.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));

            }
        }

        [HttpPatch("updatePurchaseOrderStatus/{id}")]
        [ProducesResponseType(typeof(PurchaseOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] int statusId, int id)
        {
            try
            {
                var updatedOrder = await _purchaseOrderService.UpdateOrderStatusAsync(id, statusId);
                if (updatedOrder == null)
                {
                    return NotFound(Array.Empty<object>());
                }

                return Ok(new
                {
                    message = "Purchase Order Status Updated Successfully",
                    result = updatedOrder,
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
                _logger.LogError(ex, "Error occurred while updating an order.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }

    }
}
