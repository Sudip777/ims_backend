using inventory_management_system.Repository.Implementations;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Implementations;
using inventory_management_system.Services.Interfaces;


namespace inventory_management_system.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IProductSupplierRepository, ProductSupplierRepository>();
            services.AddScoped<IProductSupplierService, ProductSupplierService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IInventoryTransactionHistoryRepository, InventoryTransactionHistoryRepository>();
            services.AddScoped<IInventoryTransactionHistoryService, InventoryTransactionHistoryService>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IUrlEndpointRepository, UrlEndpointRepository>();
            services.AddScoped<IUrlEndpointService, UrlEndpointService>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<IOnboardingRepository, OnboardingRepository>();
            services.AddScoped<IOnboardingService, OnboardingService>();
            services.AddScoped<IPurchaseOrderStatusRepository, PurchaseOrderStatusRepository>();
            services.AddScoped<IPurchaseOrderStatusService, PurchaseOrderStatusService>();
            services.AddScoped<IUserMenuRepository, UserMenuRepository>();
            services.AddScoped<IUserMenuService, UserMenuService>();
            services.AddScoped<IPurchaseOrderStatusService, PurchaseOrderStatusService>();
            services.AddScoped<ITokenCleanupService, TokenCleanupService>();
            services.AddHostedService<TokenCleanupService>();
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
