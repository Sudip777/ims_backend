using Azure;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Helpers;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace inventory_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
              public AccountController(
              ApplicationDBContext context,
              IConfiguration configuration,
              ILogger<AccountController> logger,
              IUserService userService)
                {
                    _context = context;
                    _configuration = configuration;
                    _logger = logger;
                    _userService = userService;
                }



        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDTO)
        {
            var user = await _userService.ValidateLoginAsync(loginDTO);

            if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, loginDTO.Password))
                return Unauthorized(new { Message = "Invalid username or password" });

            var claims = JwtHelpers.CreateClaims(user, _configuration);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn
            );
            var refreshToken = new JwtSecurityToken(
               _configuration["Jwt:Issuer"],
               _configuration["Jwt:Audience"],
               claims,
               expires: DateTime.UtcNow.AddMinutes(15),
               signingCredentials: signIn
           );

            var refreshTokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);
            JwtHelpers.SetRefreshTokenCookie(Response, refreshTokenString);

            return Ok(new
            {
                result = new
                {
                    access_token = new JwtSecurityTokenHandler().WriteToken(token),
                    refresh_token = refreshTokenString
                }
            });

        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }

            try
            {
                var userResponse = await _userService.RegisterAsync(dto);

                var response = new
                {
                    message = "User Added Successfully",
                    result = userResponse,
                    response_code = "00"
                };

                return Ok(response);

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a user.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext));
            }
        }




        [HttpGet("me")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("User ID claim missing or invalid");
            }

            var response = await _userService.GetCurrentUserAsync(userId);
            if (response == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return Ok(new
            {
                message = "User Fetched Successfully",
                result = response,
                response_code = "00"
            });
        }





        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { Message = "Refresh token is missing" });

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
                }, out var validatedToken);

                if (validatedToken is not JwtSecurityToken)
                    return BadRequest(new { Message = "Invalid refresh token" });

                var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                    return Unauthorized(new { Message = "Invalid user ID in token" });

                var user = await _userService.ValidateRefreshTokenUserAsync(userId);

                if (user == null)
                    return NotFound(new { Message = "User not found" });

                //  new access token
                var claims = JwtHelpers.CreateClaims(user, _configuration);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var newAccessToken = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: signIn
                );

                var newRefreshToken = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: signIn
                );
                var newRefreshTokenString = tokenHandler.WriteToken(newRefreshToken);
                JwtHelpers.SetRefreshTokenCookie(Response, newRefreshTokenString);

                return Ok(new
                {
                    result = new
                    {
                        access_token = tokenHandler.WriteToken(newAccessToken),
                        refresh_token = newRefreshTokenString
                    }
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token.");
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ExceptionHandler.ErrorHandler.HandleException(ex, HttpContext)
                );
            }
        }
    }



}
