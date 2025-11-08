using inventory_management_system.Constants;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Exceptions;
using inventory_management_system.Models;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route(ApiRoutes.Onboarding.Base)]
    public class OnboardingController:ControllerBase
    {
        private readonly ILogger<OnboardingController> _logger;
        private readonly ApplicationDBContext _context;
        private readonly IOnboardingService _service;
        public OnboardingController(ILogger<OnboardingController> logger, ApplicationDBContext context, IOnboardingService service)
        {
            _logger = logger;
            _context = context;
            _service = service;
        }
        [HttpPost]
        [ProducesResponseType(typeof(Onboarding), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOnboardingDetails([FromBody] OnboardingDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Onboarding data is required." });

            try
            {
                var data = await _service.CreateOnboardingAsync(dto); 

                _logger.LogInformation("New onboarding created with ID {OnboardingId}", data.OnboardinigId);

                return Ok(new
                {
                    message = "Onboarding Created Successfully",
                    result = data,
                    response_code = "00"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating onboarding");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while creating onboarding");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext));
            }
        }
    }
}
