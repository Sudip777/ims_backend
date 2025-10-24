using System.ComponentModel;

namespace inventory_management_system.Enums
{
    public enum UserRole
    {
        [Description("Admin")]
        ADMIN = 1,

        [Description("Manager")]
        MANAGER = 4,

        [Description("Sales")]
        SALES = 5,

        [Description("Warehouse")]
        WAREHOUSE = 6,

        [Description("Support")]
        SUPPORT = 7,

        [Description("Super Admin")]
        SUPER_ADMIN = 8
    }
}
