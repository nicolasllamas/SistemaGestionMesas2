using SistemaGestionMesas2.Data;
using SistemaGestionMesas2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionMesas2.Repository
{
    public class TableProductRepository
    {
        private readonly ApplicationDbContext _context;

        public TableProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddTableProduct(TableProduct tableProduct) // Create a new TableProduct
        {
            _context.TableProduct.Add(tableProduct);
            _context.SaveChanges();
        }
        public void ChangeQuantity(int tableId, int productId, int quantity)
        {
            var tableProduct = _context.TableProduct
                .Where(tp => tp.TableId == tableId && tp.ProductId == productId)
                .FirstOrDefault();
            if (tableProduct != null)
            {
                tableProduct.Quantity = quantity;
                _context.SaveChanges();
            }
        }
        public void RemoveAllProductsFromTable(int tableId)
        {
            var tableProducts = _context.TableProduct
                .Where(tp => tp.TableId == tableId)
                .ToList();

            if (tableProducts.Any())
            {
                _context.TableProduct.RemoveRange(tableProducts);
                _context.SaveChanges();
            }
        }

    }
}
