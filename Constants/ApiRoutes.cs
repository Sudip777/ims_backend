namespace inventory_management_system.Constants
{
    public class ApiRoutes
    {
        /// <summary>
        /// Centralized API route constants for the entire application.
        /// Use Base routes on controllers with [Route(ApiRoutes.ControllerName.Base)]
        /// and relative paths on actions with [HttpGet("relative-path")]
        /// </summary>

        #region Auth Routes
        public static class Auth
        {
            public const string Base = "api/auth";
            public const string Login = "login";
            public const string Register = "register";
            public const string RefreshToken = "refresh-token";
            public const string Logout = "logout";
            public const string ChangePassword = "change-password";
            public const string ForgotPassword = "forgot-password";
            public const string ResetPassword = "reset-password";
        }
        #endregion

        #region Customer Routes
        public static class Customers
        {
            public const string Base = "api/customers";
            public const string ById = "{id}";
            public const string Search = "search";
            public const string Active = "active";
            public const string Activate = "{id}/activate";
            public const string Deactivate = "{id}/deactivate";
            public const string Orders = "{id}/orders";
            public const string OrderHistory = "{id}/order-history";
        }
        #endregion

        #region Inventory Routes
        public static class Inventory
        {
            public const string Base = "api/inventory";
            public const string ById = "{id}";
            public const string ByProduct = "by-product/{productId}";
            public const string LowStock = "low-stock";
            public const string OutOfStock = "out-of-stock";
            public const string Adjust = "adjust";
            public const string AdjustById = "{id}/adjust";
            public const string History = "{id}/history";
        }
        #endregion

        #region Order Routes
        public static class Orders
        {
            public const string Base = "api/orders";
            public const string ById = "{id}";
            public const string ByCustomer = "by-customer/{customerId}";
            public const string Items = "{id}/items";
            public const string ItemById = "{id}/items/{itemId}";
            public const string Status = "{id}/status";
            public const string Pending = "pending";
            public const string Completed = "completed";
            public const string Cancelled = "cancelled";
            public const string Cancel = "{id}/cancel";
            public const string Complete = "{id}/complete";
        }
        #endregion

        #region Product Routes
        public static class Products
        {
            public const string Base = "api/products";
            public const string ById = "{id}";
            public const string Search = "search";
            public const string ByCategory = "by-category/{categoryId}";
            public const string BySKU = "by-sku/{sku}";
            public const string Active = "active";
            public const string Activate = "{id}/activate";
            public const string Deactivate = "{id}/deactivate";
            public const string Suppliers = "{id}/suppliers";
            public const string Stock = "{id}/stock";
            public const string LowStock = "low-stock";
            public const string UpdatePrice = "{id}/price";
        }
        #endregion

        #region Purchase Order Routes
        public static class PurchaseOrders
        {
            public const string Base = "api/purchaseorders";
            public const string ById = "{id}";
           
        }
        #endregion

        #region Role Routes
        public static class Roles
        {
            public const string Base = "api/roles";
            public const string ById = "{id}";
          
        }
        #endregion

        #region Supplier Routes
        public static class Suppliers
        {
            public const string Base = "api/suppliers";
            public const string ById = "{id}";
            public const string Search = "search";
            public const string Active = "active";
            public const string Activate = "{id}/activate";
           
        }
        #endregion

        #region Product-Supplier Routes
        public static class ProductSuppliers
        {
            public const string Base = "api/productsuppliers";
            public const string ById = "{id}";


        }
        #endregion

        #region Category Routes
        public static class Categories
        {
            public const string Base = "api/categories";
            public const string ById = "{id}";
            public const string Products = "{id}/products";
        }
        #endregion

        #region Dashboard Routes
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
        #endregion

        
    }
}
