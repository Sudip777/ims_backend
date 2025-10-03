using System.ComponentModel;

namespace inventory_management_system.Enums
{
    public enum UserRole
    {
        [Description("Admin")]
        Admin = 1,

        [Description("Manager")]
        Manager = 4,

        [Description("Sales")]
        Sales = 5,

        [Description("Warehouse")]
        Warehouse = 6,

        [Description("Support")]
        Support = 7
    }
}
