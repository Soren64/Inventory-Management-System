using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StockManager.Models;
using StockManager.Data; 

namespace StockManager.Services
{
    public class ProductService
    {
        private readonly StockContext _context;

        public ProductService(StockContext context)
        {
            _context = context;

            // Ensure database is created
            _context.Database.EnsureCreated();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            Console.WriteLine($"Added product: {product.Name} (ID: {product.Id})");
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            Console.WriteLine($"Updated product: {product.Name} (ID: {product.Id})");
        }

        public void DeleteProduct(int id)
        {
            var existing = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existing != null)
            {
                _context.Products.Remove(existing);
                _context.SaveChanges();
                Console.WriteLine($"Deleted product: {existing.Name} (ID: {existing.Id})");
            }
            else
            {
                Console.WriteLine("DeleteProduct(): Product not found.");
            }
        }
    }
}