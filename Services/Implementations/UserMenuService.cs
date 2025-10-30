using inventory_management_system.DTOs.Responses;
using inventory_management_system.Enums;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;

namespace inventory_management_system.Services.Implementations
{
    public class UserMenuService : IUserMenuService
    {
        private readonly IUserMenuRepository _repository;

        public UserMenuService(IUserMenuRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<NavItemDto>> GetMenuItemsAsync(int roleId)
        {
            var urls = await _repository.GetMappedUrlsByRoleIdAsync(roleId);

            // Ensure "/dashboard/overview" is included for Admin and Super Admin
            if (roleId == (int)UserRole.ADMIN || roleId == (int)UserRole.SUPER_ADMIN)
            {
                if (!urls.Contains("/dashboard/overview"))
                    urls.Add("/dashboard/overview");

                // Always put it first
                urls = urls
                    .OrderByDescending(u => u == "/dashboard/overview")
                    .ThenBy(u => u)
                    .ToList();
            }

            Console.WriteLine($"Role {roleId} URLs: {string.Join(", ", urls)}");

            return urls.Select(MapUrlToNavItem).ToList();
        }

        private NavItemDto MapUrlToNavItem(string url)
        {
            return url switch
            {
                "/dashboard/overview" => new NavItemDto { Icon = "dashboard-overview", Label = "Overview", Route = url },
                "/dashboard/category" => new NavItemDto { Icon = "category", Label = "Category", Route = url },
                "/dashboard/inventory" => new NavItemDto { Icon = "stock", Label = "Inventory", Route = url },
                "/dashboard/purchase-order" => new NavItemDto { Icon = "purchase-order", Label = "Purchase Order", Route = url },
                "/dashboard/sales-order" => new NavItemDto { Icon = "sales-order", Label = "Sales Order", Route = url },
                "/dashboard/customer" => new NavItemDto { Icon = "customer", Label = "Customer", Route = url },
                "/dashboard/warehouse" => new NavItemDto { Icon = "dashboard-warehouse", Label = "Warehouse", Route = url },
                "/dashboard/product" => new NavItemDto { Icon = "product", Label = "Product", Route = url },
                "/dashboard/supplier" => new NavItemDto { Icon = "supplier", Label = "Supplier", Route = url },
                _ => new NavItemDto { Icon = "default", Label = "Unknown", Route = url }
            };
        }
    }
}
