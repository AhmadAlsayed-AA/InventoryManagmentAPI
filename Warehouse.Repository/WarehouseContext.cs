using System;
using Warehouse.Data;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.ProductModels;
using Warehouse.Data.UserModels;
using Warehouse.Data.OrderModels;
using Warehouse.Data.CustomerModels;

namespace Warehouse.Repository
{
	public class WarehouseContext : DbContext
    {
		public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }


        public WarehouseContext()
		{
		}
        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=Hesham_Omen15\\SQLEXPRESS;Initial Catalog=WarehouseDB;Integrated Security=True;Pooling=False;TrustServerCertificate=True;");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>()
                .HasKey(bc => new { bc.OrderId, bc.ProductId });
            modelBuilder.Entity<OrderProduct>()
                .HasOne(bc => bc.Order)
                .WithMany(b => b.OrderProducts)
                .HasForeignKey(bc => bc.OrderId);
            modelBuilder.Entity<OrderProduct>()
                .HasOne(bc => bc.Product)
                .WithMany(c => c.OrderProducts)
                .HasForeignKey(bc => bc.ProductId);
        }
    }
}

