namespace inventory_management_system.Constants
{
    public class ApiRoutes
    {
        /// <summary>
        /// Centralized API route constants for the entire application.
        /// Use Base routes on controllers with [Route(ApiRoutes.ControllerName.Base)]
        /// and relative paths on actions with [HttpGet("relative-path")]
        /// </summary>

        public static class Auth
        {
            public const string Base = "api/auth";
            public const string Login = "login";
            public const string Register = "register";
            public const string RefreshToken = "refresh-token";
            public const string Logout = "logout";
           
           
        }
        public static class User
        {
            public const string Base = "user";
            public const string ById = "{id}";
        }


        public static class Customers
        {
            public const string Base = "api/customers";
            public const string ById = "{id}";
        }
       

        public static class Inventory
        {
            public const string Base = "api/inventories";
            public const string ById = "{id}";
            public const string LowStock = "low-stock";
          
        }
       

        public static class Orders
        {
            public const string Base = "api/orders";
            public const string ById = "{id}";
        }
       

        public static class Products
        {
            public const string Base = "api/products";
            public const string ById = "{id}";
            public const string LowStock = "low-stock";
        }
       

        public static class PurchaseOrders
        {
            public const string Base = "api/purchaseorders";
            public const string ById = "{id}";
           
        }
       

      
        public static class Roles
        {
            public const string Base = "api/roles";
            public const string ById = "{id}";
          
        }
       

        public static class Suppliers
        {
            public const string Base = "api/suppliers";
            public const string ById = "{id}";
           
        }
       

        public static class ProductSuppliers
        {
            public const string Base = "api/productsuppliers";
            public const string ById = "{id}";


        }

        public static class Warehouses
        {
            public const string Base = "api/warehouses";
            public const string ById = "{id}";
        }
        public static class Categories
        {
            public const string Base = "api/categories";
            public const string ById = "{id}";
        }
        public static class InventoryTransactionHistory
        {
            public const string Base = "api/inventorytransactionhistories";
            public const string ById = "{id}";
        }
        public static class RolePermissions
        {
            public const string Base = "api/role-permission";
            public const string ById = "{id}";

        }

        public static class UrlEndpoints
        {
            public const string Base = "api/url-endpoints";
            public const string ById = "{id}";
        }
        public static class PurchaseOrderStatus
        {
            public const string Base = "api/purchase-order-status";
            public const string ById = "{id}";
        }
        public static class UserMenu
        {
            public const string Base = "api/user-menu";
            public const string ById = "{id}";
        }

        public static class Dashboard
        {
            public const string Base = "api/dashboard";
            public const string InventoryOverview = "inventory-overview";
            public const string InventoryTransactinOverview = "inventory-transaction-overview";
            public const string Warehouse = "warehouse-overview";
            public const string Sales = "sales-performance";
            public const string Summary = "purchase-order-status";
            public const string Orders = "orders";
            public const string Revenue = "revenue";
            public const string TopProducts = "top-products";
            public const string TopCustomers = "top-customers";
        }
    }
}
