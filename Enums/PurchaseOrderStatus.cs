using System.ComponentModel;

namespace inventory_management_system.Enums
{
    public enum PurchaseOrderStatus
    {
        [Description("Pending")]
        Pending = 1,

        [Description("Processed")]
        Processed = 2,

        [Description("Cancelled")]
        Cancelled = 3
    }
}
