namespace inventory_management_system.Constants
{
  
        public static class BusinessConstants
        {
            // Inventory Management
            public static class Inventory
            {
                public const int LOW_STOCK_THRESHOLD = 5;
                public const int CRITICAL_STOCK_THRESHOLD = 2;
                public const decimal DEFAULT_MARKUP_PERCENTAGE = 0.30m; // 30% markup
            }

            // Product Management
            public static class Products
            {
                public const decimal MIN_PRICE = 0.01m;
                public const decimal MAX_PRICE = 999999.99m;
                public const int DEFAULT_WARRANTY_MONTHS = 12;
            }

            // Order Management
            public static class Orders
            {
                public const int DEFAULT_ORDER_TIMEOUT_MINUTES = 30;
                public const decimal MIN_ORDER_AMOUNT = 1.00m;
            }
        }
}
