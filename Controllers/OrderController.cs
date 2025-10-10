using FluentValidation;
using inventory_management_system.Constants;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Exceptions;
using inventory_management_system.Extensions;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [Route(ApiRoutes.Orders.Base)]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly IValidator<OrderDto> _validator;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService, IValidator<OrderDto> validator)
        {
            _logger = logger;
            _orderService = orderService;
            _validator = validator;
        }


        [HttpPost]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            var result = await _validator.ValidateAsync(orderDto);
            if (!result.IsValid)
            {
                return this.ValidationProblem(result);

            }
            try
            {
                var createdOrder = await _orderService.CreateOrderAsync(orderDto);
                return Ok(new
                {
                    message = "Order Created Successfully",
                    result = createdOrder,
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
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrdersRequest request)
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync(request);

                if(orders.Data==null || !orders.Data.Any())
                {
                    return NotFound(new
                    {
                        message = "No Orders Found.",
                    });
                }
                return Ok(new
                {
                    message = "Orders Rtrieved Successfully",
                    result = orders,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving orders.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }


        [HttpGet(ApiRoutes.Orders.ById)]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var data = await _orderService.GetOrderByIdAsync(id);
                return Ok(new
                {
                    message = "Order Retrieved Successfully",
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
                _logger.LogError(ex, "Error occurred while retrieving the order.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }

       


        [HttpPut(ApiRoutes.Orders.ById)]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDto dto , int id)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                return this.ValidationProblem(result);

            }
            try
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(dto, id);
                return Ok(new
                {
                    message = "Inventory Updated Successfully",
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
                   ExceptionHandler.HandleException(ex, HttpContext));

            }

        }


        [HttpPatch(ApiRoutes.Orders.ById)]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] int id, [FromBody] int statusId)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, statusId);

                return Ok(new
                {
                    message = "Order Status Updated Successfully",
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
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }
    }
}
//[HttpDelete("deleteOrderById/{id}")]
//public async Task<IActionResult> DeleteOrder([FromRoute] int id)
//{
//    try
//    {
//        await _orderService.DeleteOrderAsync(id);

//        return Ok(new
//        {
//            message = "Order Deleted Successfully",
//            response_code = "00"
//        });
//    }
//    catch (InvalidOperationException ex)
//    {
//        return BadRequest(new { message = ex.Message });
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, "Error occurred while retrieving the order.");
//        return StatusCode(StatusCodes.Status500InternalServerError,
//            ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
//    }

//}
