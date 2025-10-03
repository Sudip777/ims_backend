using System.ComponentModel;

namespace inventory_management_system.Enums
{
        public enum TransactionType
        {
        [Description("Sale")]
        Sale = 1,
        [Description("Purchase")]
        Purchase = 2,
        [Description("Adjustment")]
        Adjustment = 3,
        [Description("Return")]
        Return = 4,
        [Description("Transfer")]
        Transfer = 5
    }

}
