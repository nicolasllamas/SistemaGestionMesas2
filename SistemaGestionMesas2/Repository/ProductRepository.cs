using Microsoft.EntityFrameworkCore;
using SistemaGestionMesas2.Data;
using SistemaGestionMesas2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionMesas2.Repository
{
    public class ProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Product> GetAllProducts() // Returns all products, including the products in them
        {
            return _context.Product.Include(t => t.TableProducts);
        }

        public void AddProduct(string name, decimal price)
        {
            Product product = new Product(name, price);
            _context.Product.Add(product);
            _context.SaveChanges();
        }

        public void AddProduct(Product product) //Overload, not used in the project. Might be useful in the future
        {
            _context.Product.Add(product);
            _context.SaveChanges();
        }
        public Product GetProductById(int id) // Find a existing table (!) Should only be used if you are not sure that the table exists
        {
            return _context.Product.Find(id);
        }

        public void RemoveProduct(Product product) //Remove a product
        {
            _context.Product.Remove(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product) //Update a table
        {
            _context.Product.Update(product);
            _context.SaveChanges();
        }
    }
}
