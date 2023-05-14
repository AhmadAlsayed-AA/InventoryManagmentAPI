using System;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Product;
using Warehouse.Repository;
using static Warehouse.Services.ProductService;

namespace Warehouse.Services
{
    public interface IProductService
    {
        public List<Product> GetAllProducts();
        public Product GetProductById(int id);
        public void AddProduct(Product product);
        public Product UpdateProduct(Product product);
        public void DeleteProduct(Product product);
        

    }
    public class ProductService : IProductService
    {
        private readonly WarehouseContext _context;

        public ProductService(WarehouseContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.AsNoTracking().Include(i => i.Category).ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.AsNoTracking().Include(i => i.Category).FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public Product UpdateProduct(Product product)
        {
            var originalProduct = _context.Products.Find(product.Id);
            if (originalProduct == null)
            {
                throw new ArgumentException("Category not found");
            }

            var properties = typeof(Category).GetProperties();
            foreach (var property in properties)
            {
                var newValue = property.GetValue(product);
                if (newValue != null)
                {
                    property.SetValue(originalProduct, newValue);
                }
            }

            _context.SaveChanges();
            return originalProduct;

        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }


}



