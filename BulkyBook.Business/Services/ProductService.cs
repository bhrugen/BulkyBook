using BulkyBook.Business.Services.IServices;
using BulkyBook.Models;
using BulkyBookWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;    
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product {id} not found");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsProductNameUniqueAsync(string name, int? productId = null)
        {
            if (productId.HasValue)
            {
                return !await _context.Products.AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != productId.Value);
            }
            else
            {
                return !await _context.Products.AnyAsync(c => c.Name.ToLower() == name.ToLower());
            }
        }
    }
}
