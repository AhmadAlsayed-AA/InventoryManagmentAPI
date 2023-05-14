using System;
using Warehouse.Data;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Product;

namespace Warehouse.Repository
{
	public class WarehouseContext : DbContext
    {
		public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public WarehouseContext()
		{
		}
        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Database=WarehouseDB; User Id=sa; Password=Strong.Pwd-123;TrustServerCertificate=True;");

        }
    }
}

