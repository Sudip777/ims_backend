using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Exceptions;
using inventory_management_system.Models;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController: ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }


        [HttpGet("getAllInventories")]
        public async Task<IActionResult> GetAllInventories()
        {
            try
            {
                var inventories = await _inventoryService.GetAllInventoryAsync();

                return Ok(new
                {
                    message = "Inventories retrieved successfully",
                    result = inventories,
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


        [HttpGet("getAllInventories/{id}")]
        public async Task<IActionResult> GetInventoryById(int id)
        {

           
            try
            {
                var data = await _inventoryService.GetInventoryByIdAsync(id);

                return Ok(new
                {
                    message = "Inventory retrieved successfully",
                    result = data,
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


        [HttpPost("createInventory")]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryDto dto)
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
                var createdInventory = await _inventoryService.CreateInventoryAsync(dto);
                return Ok(new
                {
                    message = "Inventory Created Successfully",
                    result = createdInventory,
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

        [HttpPut("updateInventory{id}")]
        public async Task<IActionResult> UpdateInventory([FromBody] InventoryDto dto, int id)
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
                var updatedInventory = await _inventoryService.UpdateInventoryAsync(dto, id);
                return Ok(new
                {
                    message = "Inventory Updated Successfully",
                    result = updatedInventory,
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

        [HttpGet("lowStocks")]
        public async Task<IActionResult> GetLowStocks()
        {
            try
            {
                var lowStockItems = await _inventoryService.GetLowStocks() ?? new List<InventoryResponse>(); ;
                return Ok(new
                {
                    message = "Low stock items retrieved successfully",
                    result = lowStockItems,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving low stock items.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }




    }
}

