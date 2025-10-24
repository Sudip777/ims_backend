using FluentValidation;
using inventory_management_system.Constants;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Exceptions;
using inventory_management_system.Extensions;
using inventory_management_system.Filters;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{

    [Route(ApiRoutes.Inventory.Base)]
    [ApiController]
    [Authorize]
    [RolePermission]
    public class InventoryController: ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;
        private readonly IValidator<InventoryDto> _validator;
        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger, IValidator<InventoryDto> validator)
        {
            _inventoryService = inventoryService;
            _logger = logger;
            _validator = validator;
        }


        [HttpGet]
        [ProducesResponseType(typeof(InventoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllInventories([FromQuery] GetAllInventoriesRequest request)
        {
            try
            {
                var inventories = await _inventoryService.GetAllInventoryAsync(request);
                if (inventories.Data == null || !inventories.Data.Any())
                {
                    return NotFound(new
                    {
                        message = "No Inventories Found.",
                    });
                }
                return Ok(new
                {
                    message = "Inventories Retrieved Successfully.",
                    result = new { data = inventories.Data, meta = inventories.Meta },
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
                $"Unhandled exception while retriving inventories, by user {User.Identity?.Name}",
                User.Identity?.Name ?? "Anonymous"
                );
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpGet(ApiRoutes.Inventory.ById)]
        [ProducesResponseType(typeof(InventoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInventoryById(int id)
        {
            try
            {
                var data = await _inventoryService.GetInventoryByIdAsync(id);

                return Ok(new
                {
                    message = "Inventory Retrieved Successfully.",
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
                _logger.LogError(ex,
               $"Unhandled exception while retriving inventory of{id}, by user {User.Identity?.Name}",id,
               User.Identity?.Name ?? "Anonymous"
               );
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(InventoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryDto dto)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for inventory creation: Errors: {@Errors}",
                result.Errors.Select(e => e.ErrorMessage));
                return this.ValidationProblem(result);

            }

            try
            {
                var createdInventory = await _inventoryService.CreateInventoryAsync(dto);
                return Ok(new
                {
                    message = "Inventory Created Successfully.",
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
                _logger.LogError(ex,
               $"Unhandled exception while creating inventory, by user {User.Identity?.Name}",
               User.Identity?.Name ?? "Anonymous"
               );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpPut(ApiRoutes.Inventory.ById)]
        [ProducesResponseType(typeof(InventoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInventory([FromBody] InventoryDto dto, int id)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for inventory update: Errors: {@Errors}",
                  result.Errors.Select(e => e.ErrorMessage));
                return this.ValidationProblem(result);

            }
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
                    message = "Inventory Updated Successfully.",
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
                _logger.LogError(ex,
             $"Unhandled exception while updaaating inventory, by user {User.Identity?.Name}",
             User.Identity?.Name ?? "Anonymous"
             );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));

            }
        }

        [HttpGet(ApiRoutes.Inventory.LowStock)]
        [ProducesResponseType(typeof(InventoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLowStocks()
        {
            try
            {
                var lowStockItems = await _inventoryService.GetLowStocks() ?? new List<InventoryResponse>(); ;
                return Ok(new
                {
                    message = "Low Stock Items Retrieved Successfully.",
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
                _logger.LogError(ex,
             $"Unhandled exception while retrieving low stocks inventory, by user {User.Identity?.Name}",
             User.Identity?.Name ?? "Anonymous"
             );
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }




    }
}

