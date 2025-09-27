using inventory_management_system.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace inventory_management_system.Helpers
{
    public class JwtHelpers
    {
        
        public static List<Claim> CreateClaims(User user, IConfiguration configuration)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("Username", user.Username),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };
        }

        public static void SetRefreshTokenCookie(HttpResponse response, string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to true in production for HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}

