using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.Models
{
    public class TransactionType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionTypeId { get; set; }

        [Required]
        [StringLength(50)]
        [Index("IX_TransactionTypes_Name", IsUnique = true)]
        public string Name { get; set; }

        // Navigation property
        public virtual ICollection<InventoryTransactionHistory> Transactions { get; set; }
    }

}
