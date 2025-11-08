namespace inventory_management_system.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    
        public class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int UserId { get; set; }

            [Required]
            [StringLength(100)]
            public required string Username { get; set; }

            [Required]
            [StringLength(255)]
            public required string PasswordHash { get; set; }

            

            [StringLength(150)]
            public required string FullName { get; set; }

            [Required]
            [StringLength(150)]
            public required string Email { get; set; }

            [Required]
            public int RoleId { get; set; }

            [ForeignKey("RoleId")]
            public  virtual Role? Role { get; set; }

            public DateTime? LastLogin { get; set; }

            [Required]
            public bool IsActive { get; set; } = true;

            [Required]
            public DateTime CreatedAt { get; set; } = DateTime.Now;
            public int? OnboardingId { get; set; }
        [JsonIgnore]
        [ForeignKey("OnboardingId")]
            public virtual Onboarding? Onboarding { get; set; }

        // Navigation properties
            public virtual ICollection<Customer>? CreatedCustomers { get; set; }
            public virtual ICollection<Supplier>? CreatedSuppliers { get; set; }
            public virtual ICollection<Warehouse>? CreatedWarehouses { get; set; }
            public virtual ICollection<Order>? CreatedOrders { get; set; }
            public virtual ICollection<PurchaseOrder>? CreatedPurchaseOrders { get; set; }
            public virtual ICollection<InventoryTransactionHistory>? Transactions { get; set; }
        }
}
