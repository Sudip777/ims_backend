using inventory_management_system.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

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

        // SUPER_ADMIN bypass
        var isSuperAdmin = dbContext.Roles
            .Any(r => r.RoleId == roleId && r.RoleName == "SUPER_ADMIN");
        if (isSuperAdmin)
            return;

        var requestPath = context.HttpContext.Request.Path.Value ?? string.Empty;
        var requestMethod = context.HttpContext.Request.Method.ToUpperInvariant();

        var cacheKey = $"Permission_{userId}_{requestMethod}_{requestPath}";
        if (!cache.TryGetValue(cacheKey, out bool hasPermission))
        {
            var normalizedPath = NormalizePath(requestPath);

            // Log for debugging
            logger.LogInformation("Checking permission for RoleId={RoleId}, Method={Method}, NormalizedPath={Path}",
                roleId, requestMethod, normalizedPath);

            hasPermission = dbContext.RolePermissions
                 .Include(rp => rp.UrlEndpoint)
                 .Include(rp => rp.Method)
                 .Where(rp => rp.RoleId == roleId)
                 .ToList() // Bring to memory first
                 .Any(rp =>
                 {
                     var dbPath = NormalizePath(rp.UrlEndpoint.Url);
                     var methodMatches = string.Equals(rp.Method.MethodName, requestMethod, StringComparison.OrdinalIgnoreCase);
                     var pathMatches = dbPath == normalizedPath;

                     // Log EVERY comparison
                     logger.LogInformation("RolePermission Check: RoleId={RoleId}, DB URL='{DbUrl}' -> Normalized='{DbPath}', Method='{DbMethod}' | Request: Path='{RequestPath}', Method='{RequestMethod}' | Match: Path={PathMatch}, Method={MethodMatch}",
                         roleId, rp.UrlEndpoint.Url, dbPath, rp.Method.MethodName, normalizedPath, requestMethod, pathMatches, methodMatches);

                     return pathMatches && methodMatches;
     });

            cache.Set(cacheKey, hasPermission, TimeSpan.FromMinutes(10));
        }

        if (!hasPermission)
        {
            logger.LogWarning("User {UserId} with RoleId {RoleId} denied access to {Method} {Path}",
                userId, roleId, requestMethod, requestPath);
            context.Result = new ForbidResult();
        }
    }

    private static string NormalizePath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return string.Empty;

        path = path.Trim().ToLowerInvariant();

        // Ensure leading slash first
        if (!path.StartsWith("/"))
            path = "/" + path;

        // NOW remove /api prefix
        if (path.StartsWith("/api/"))
            path = path.Substring(4); // Remove "/api"

        // Remove trailing slash
        path = path.TrimEnd('/');

        return path;
    }

}