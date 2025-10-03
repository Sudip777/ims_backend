using System.ComponentModel;

namespace inventory_management_system.Enums
{
   
        public enum OrderStatus
        {
            [Description("Pending")]
            Pending = 1,

            [Description("Processing")]
            Processing = 2,

            [Description("Shipped")]
            Shipped = 3,

            [Description("Completed")]
            Completed = 4,

            [Description("Cancelled")]
            Cancelled = 5
        }
    }

