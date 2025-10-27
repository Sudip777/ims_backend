using FluentValidation;
using inventory_management_system.Constants;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Exceptions;
using inventory_management_system.Extensions;
using inventory_management_system.Helpers;
using inventory_management_system.Models;
using inventory_management_system.Security;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace inventory_management_system.Controllers
{
    [Route(ApiRoutes.Auth.Base)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IValidator<RegisterUserDto> _validator;
        public AccountController(
        IValidator<RegisterUserDto> validator,
        ApplicationDBContext context,
        IConfiguration configuration,
        ILogger<AccountController> logger,
        IUserService userService)
        {
            _validator = validator;
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost(ApiRoutes.Auth.Login)]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDTO)
        {
            var user = await _userService.ValidateLoginAsync(loginDTO);

            if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, loginDTO.Password))
                return Unauthorized(new { Message = "Invalid username or password" });

            var claims = JwtHelpers.CreateClaims(user, _configuration);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: signIn
            );

            var refreshToken = new JwtSecurityToken(
               _configuration["Jwt:Issuer"],
               _configuration["Jwt:Audience"],
               claims,
               expires: DateTime.UtcNow.AddMinutes(240),
               signingCredentials: signIn
           );

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessTokenString = tokenHandler.WriteToken(accessToken);
            var refreshTokenString = tokenHandler.WriteToken(refreshToken);

            // Store refresh token in DB
            var tokenEntity = new Token
            {
                UserId = user.UserId,
                TokenDetail = refreshTokenString,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = refreshToken.ValidTo,
                User = user
            };
            _context.Tokens.Add(tokenEntity);
            await _context.SaveChangesAsync();

            //Set secure HTTP-only cookie for refresh token
            JwtHelpers.SetRefreshTokenCookie(Response, refreshTokenString);

            return Ok(new
            {
                result = new
                {
                    access_token = accessTokenString,
                    refresh_token = refreshTokenString
                }
            });

        }

        [HttpPost(ApiRoutes.Auth.Register)]
        [Authorize]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                return this.ValidationProblem(result);
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
                   ExceptionHandler.HandleException(ex, HttpContext));
            }
        }




        [HttpGet(ApiRoutes.User.Base)]
        [Authorize]
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

        [HttpPost(ApiRoutes.Auth.RefreshToken)]
        [Authorize]
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
                    ValidateLifetime = true, //// ensures expired tokens are rejected
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

                // Check if refresh token exists and is valid in DB
                var storedToken = await _context.Tokens
                .FirstOrDefaultAsync(t => t.UserId == userId && t.TokenDetail == refreshToken);

                if (storedToken == null || storedToken.ExpiresAt <= DateTime.UtcNow)
                    return Unauthorized(new { Message = "Invalid or expired refresh token" });
                // Remove the old refresh token
                _context.Tokens.Remove(storedToken);

                //  new access token
                var claims = JwtHelpers.CreateClaims(user, _configuration);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var newAccessToken = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(120),
                    signingCredentials: signIn
                );

                var newRefreshToken = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(240),
                    signingCredentials: signIn
                );

                var newRefreshTokenString = tokenHandler.WriteToken(newRefreshToken);

                // Save new refresh token in database
                var tokenEntity = new Token
                {
                    UserId = userId,
                    TokenDetail = newRefreshTokenString,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = newRefreshToken.ValidTo,
                    User = user
                };

                _context.Tokens.Add(tokenEntity);

                //On-demand cleanup of expired tokens
                var expiredTokens = await _context.Tokens
                    .Where(t => t.ExpiresAt < DateTime.UtcNow)
                    .ToListAsync();

                if (expiredTokens.Any())
                    _context.Tokens.RemoveRange(expiredTokens);

                await _context.SaveChangesAsync();

                // Set cookie
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
            catch (SecurityTokenExpiredException)
            {
                return Unauthorized(new { Message = "Refresh token expired" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                     $"Unhandled exception while handling refresh token: by user {User.Identity?.Name}",
                     User.Identity?.Name ?? "Anonymous");
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ExceptionHandler.HandleException(ex, HttpContext)
                );
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "SUPER_ADMIN")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid user ID {Id} provided for deletion.", id);
                return BadRequest("Invalid user ID.");
            }

            try
            {
                await _userService.DeleteUserAsync(id);
                _logger.LogInformation("User {Id} deleted successfully by {User}", id, User.Identity?.Name);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "User {Id} not found for deletion.", id);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operation failed for user {Id}: {Message}", id, ex.Message);
                return StatusCode((int)HttpStatusCode.Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Unhandled exception while deleting user: by user {User.Identity?.Name}",
                    User.Identity?.Name ?? "Anonymous");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid user ID {Id} provided for update.", id);
                return BadRequest("Invalid user ID.");
            }

            try
            {
                await _userService.UpdateUserAsync(id, userDto);
                _logger.LogInformation("User {Id} updated successfully by {User}", id, User.Identity?.Name);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "User {Id} not found for update.", id);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operation failed for user {Id}: {Message}", id, ex.Message);
                return StatusCode((int)HttpStatusCode.Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Unhandled exception while updating user: by user {User.Identity?.Name}",
                    User.Identity?.Name ?? "Anonymous");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}

