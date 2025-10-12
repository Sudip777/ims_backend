using inventory_management_system.Models;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<InventoryTransactionHistory> InventoryTransactions { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<UrlEndpoint> UrlEndpoints { get; set; }
        public DbSet<Method> Methods { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
            // --------------------------
            // Unique Indexes
            // --------------------------
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<RolePermission>()
            .HasIndex(rp => new { rp.RoleId, rp.UrlEndpointId, rp.MethodId })
            .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique();

            // -------------------------------------
            // Seed Method Data for RolePermissions
            // -------------------------------------
            modelBuilder.Entity<Method>().HasData(
           new Method { MethodId = 1, MethodName = "GET" },
           new Method { MethodId = 2, MethodName = "POST" },
           new Method { MethodId = 3, MethodName = "PUT" },
           new Method { MethodId = 4, MethodName = "DELETE" },
           new Method { MethodId = 5, MethodName = "PATCH" }
       );

            // --------------------------
            // Composite Indexes
            // --------------------------
            modelBuilder.Entity<InventoryTransactionHistory>()
                .HasIndex(t => new { t.ProductId, t.WarehouseId });

            modelBuilder.Entity<OrderStatus>().ToTable("OrderStatus");

            // --------------------------
            // Relationships with custom delete behavior
            // --------------------------
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.CreatedByUser)
                .WithMany(u => u.CreatedCustomers)
                .HasForeignKey(c => c.CreatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supplier>()
                .HasOne(s => s.CreatedByUser)
                .WithMany(u => u.CreatedSuppliers)
                .HasForeignKey(s => s.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.CreatedByUser)
                .WithMany(u => u.CreatedWarehouses)
                .HasForeignKey(w => w.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.CreatedByUser)
                .WithMany(u => u.CreatedOrders)
                .HasForeignKey(o => o.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.CreatedByUser)
                .WithMany(u => u.CreatedPurchaseOrders)
                .HasForeignKey(po => po.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pod => pod.PurchaseOrder)
                .WithMany(po => po.PurchaseOrderDetails)
                .HasForeignKey(pod => pod.PurchaseOrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pod => pod.Product)
                .WithMany(p => p.PurchaseOrderDetails)
                .HasForeignKey(pod => pod.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductSupplier>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSuppliers)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<ProductSupplier>()
                .HasOne(ps => ps.Supplier)
                .WithMany(s => s.ProductSuppliers)
                .HasForeignKey(ps => ps.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);


            // --------------------------
            // Check Constraints
            // --------------------------
            modelBuilder.Entity<Inventory>()
                .HasCheckConstraint("CK_Inventory_Quantity", "Quantity >= 0");

            modelBuilder.Entity<OrderDetail>()
                .HasCheckConstraint("CK_OrderDetail_Quantity", "Quantity > 0");

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasCheckConstraint("CK_PurchaseOrderDetail_Quantity", "Quantity > 0");
        }
    }
}
