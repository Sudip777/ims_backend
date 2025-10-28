namespace inventory_management_system.Helpers
{
    public class NormalizedPath
    {
        public static string NormalizePath(string path)
{
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            path = path.Trim().ToLowerInvariant();

            // Remove possible prefixes like /api
            if (path.StartsWith("/api/"))
                path = path.Substring(4);

            // Ensure consistent leading slash
            if (!path.StartsWith("/"))
                path = "/" + path;

            // Remove trailing slash
            if (path.EndsWith("/"))
                path = path.TrimEnd('/');

            return path;
}

    }
}
