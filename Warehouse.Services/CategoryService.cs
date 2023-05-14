using System;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Product;
using Warehouse.Repository;

namespace Warehouse.Services
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
        Category GetCategoryById(int id);
        void AddCategory(Category category);
        Category UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }

    public class CategoryService : ICategoryService
    {
        private readonly WarehouseContext _context;

        public CategoryService(WarehouseContext context)
        {
            _context = context;
        }

        public List<Category> GetAllCategories()
        {
            return _context.Categories.AsNoTracking().Include(i => i.Products).ToList(); ;
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.AsNoTracking().Include(i => i.Products).FirstOrDefault(c => c.Id == id);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public Category UpdateCategory(Category category)
        {
            var originalCategory = _context.Categories.Find(category.Id);
            if (originalCategory == null)
            {
                throw new ArgumentException("Category not found");
            }

            var properties = typeof(Category).GetProperties();
            foreach (var property in properties)
            {
                var newValue = property.GetValue(category);
                if (newValue != null)
                {
                    property.SetValue(originalCategory, newValue);
                }
            }

            _context.SaveChanges();
            return originalCategory;
        }

        public void DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}

