using inventory_management_system.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

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

            var userId = user.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var roleIdClaim = user.FindFirst("RoleId")?.Value;
            if (!int.TryParse(roleIdClaim, out var roleId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDBContext>();
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<RolePermissionAttribute>>();
            var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

            //Check if user is SUPER_ADMIN and allow all endpoints for him
            var isSuperAdmin = dbContext.Roles
                .Any(r => r.RoleId == roleId && r.RoleName == "SUPER_ADMIN");

            if (isSuperAdmin)
            {
                return;
            }

            // Except SUPER_ADMIN should pass this
            var requestPath = context.HttpContext.Request.Path.Value;
            var requestMethod = context.HttpContext.Request.Method;
            var cacheKey = $"Permission_{userId}_{requestMethod}_{requestPath}";

            if (!cache.TryGetValue(cacheKey, out bool hasPermission))
            {
                // Normalize the request path (avoid query parameters)
                var uri = new Uri(requestPath, UriKind.RelativeOrAbsolute);
                var normalizedPath = uri.IsAbsoluteUri ? uri.AbsolutePath : requestPath;

                hasPermission = dbContext.RolePermissions
                    .Include(rp => rp.UrlEndpoint)
                    .Include(rp => rp.Method)
                    .Any(rp =>
                        rp.RoleId == roleId &&
                        rp.UrlEndpoint.Url == normalizedPath &&
                        rp.Method.MethodName == requestMethod
                    );

                // Cache result for faster lookups
                cache.Set(cacheKey, hasPermission, TimeSpan.FromMinutes(10));
            }

            if (!hasPermission)
            {
                logger.LogWarning("User {UserId} denied access to {Method} {Path}", userId, requestMethod, requestPath);
                context.Result = new ForbidResult();
            }
        }
    }
}
