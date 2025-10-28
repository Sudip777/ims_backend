using FluentValidation;
using inventory_management_system.Constants;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Exceptions;
using inventory_management_system.Extensions;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [Route(ApiRoutes.Warehouses.Base)]
    [ApiController]
    [RolePermission]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        private readonly ILogger<WarehouseController> _logger;
        private readonly IValidator<WarehouseDto> _validator;

        public WarehouseController(IWarehouseService warehouseService, IValidator<WarehouseDto> validator)
        {
            _warehouseService = warehouseService;
            _validator = validator;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WarehouseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public  async Task<IActionResult> GetAllWarehouses()
        {

            try
            {
                var response = await _warehouseService.GetAllWarehousesAsync();
                return Ok(new
                {
                    message = "Suppliers Retrieved Successfully",
                    result = response,
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

        [HttpGet(ApiRoutes.Warehouses.ById)]
        [ProducesResponseType(typeof(IEnumerable<WarehouseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWarehouseById(int id)
        {

            try
            {
                var response = await _warehouseService.GetWarehouseByIdAsync(id);
                return Ok(new
                {
                    messaage = "Warehouse Retrieved Successfully",
                    result = response,
                    response_code = "00",
                });
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(new { Message = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,
                        $"Unhandled exception while retrieving warehouse of{id}, by user {User.Identity?.Name}",id,
                         User.Identity?.Name ?? "Anonymous"
                           ); 
                return StatusCode(StatusCodes.Status500InternalServerError,
                ExceptionHandler.HandleException(exception, HttpContext));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(WarehouseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateWarehouse([FromBody] WarehouseDto dto)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for warehouse creation: Errors: {@Errors}",
               result.Errors.Select(e => e.ErrorMessage));
                return this.ValidationProblem(result);

            }
            try
            {
                var response = await _warehouseService.CreateWarehouseAsync(dto);
                return Ok(new
                {
                    message = "Warehouse Created Successfully",
                    result = response,
                    response_code = "00",
                });
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            catch(Exception ex)
            {
                _logger.LogError(ex,
               $"Unhandled exception while creating warehouse {dto.Name}, by user {User.Identity?.Name}",dto.Name,
               User.Identity?.Name ?? "Anonymous"
               );
                return StatusCode(StatusCodes.Status500InternalServerError,
                  ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

        [HttpPut(ApiRoutes.Warehouses.ById)]
        [ProducesResponseType(typeof(IEnumerable<SupplierDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateWarehouse([FromBody] WarehouseDto  dto, int id)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for warehouse update: Errors: {@Errors}",
               result.Errors.Select(e => e.ErrorMessage));
                return this.ValidationProblem(result);

            }
            try
            {
                var response = await _warehouseService.UpdateWarehouseAsync(dto, id);
                return Ok(new
                {
                    message = "Warehouse Updated Successfully",
                    result = response,
                    response_code = "00",
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new {Message = e.Message});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                 $"Unhandled exception while updating warehouse {dto.Name}, by user {User.Identity?.Name}",dto.Name,
                 User.Identity?.Name ?? "Anonymous"
                 );
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.HandleException(ex, HttpContext));

            }

        }
    }
}
