using inventory_management_system.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace inventory_management_system.Filters
{
    public class RolePermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
      public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Bypass for admin routes
            var requestPath = context.HttpContext.Request.Path.Value;
            if (requestPath.StartsWith("/api/role-permission") || requestPath.StartsWith("/api/url-endpoints"))
            {
                return;
            }

            var requestMethod = context.HttpContext.Request.Method;
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDBContext>();
            var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            var cacheKey = $"Permission_{userId}_{requestMethod}_{requestPath}";

            if (!cache.TryGetValue(cacheKey, out bool hasPermission))
            {
                hasPermission = dbContext.RolePermissions
                    .Include(rp => rp.UrlEndpoint)
                    .Include(rp => rp.Method)
                    .Any(rp => dbContext.Users.Any(u => u.UserId.ToString() == userId && u.RoleId == rp.RoleId)
                               && (rp.UrlEndpoint.Url == requestPath)
                               && rp.Method.MethodName == requestMethod);

                cache.Set(cacheKey, hasPermission, TimeSpan.FromMinutes(10));
            }

            if (!hasPermission)
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<RolePermissionAttribute>>();
                logger.LogWarning("User {UserId} denied access to {Method} {Path}", userId, requestMethod, requestPath);
                context.Result = new ForbidResult();
            }
        }
    }
}
