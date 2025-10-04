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
       

        public static class Customers
        {
            public const string Base = "api/customers";
            public const string ById = "{id}";
        }
       

        public static class Inventory
        {
            public const string Base = "api/inventory";
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

        public static class Dashboard
        {
            public const string Base = "api/dashboard";
            public const string Summary = "summary";
            public const string Sales = "sales";
            public const string Inventory = "inventory";
            public const string Orders = "orders";
            public const string Revenue = "revenue";
            public const string TopProducts = "top-products";
            public const string TopCustomers = "top-customers";
        }
    }
}
