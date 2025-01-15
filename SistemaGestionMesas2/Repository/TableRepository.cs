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
    public class TableRepository
    {
        private readonly ApplicationDbContext _context;

        public TableRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Table> GetAllTables() // Returns all tables, including the products in them
        {
            return _context.Table.Include(t => t.TableProducts);
        }

        public Table GetTableById(int id) // Find a existing table
                                          // (!) Should only be used if you are not sure that the table exists
        {
            return _context.Table.Find(id);
        }

        public void AddTable() // Create a new table
        {
            Table table = new Table();
            _context.Table.Add(table);
            _context.SaveChanges();
        }

        public void RemoveTable(Table table) //Remove a table
        {
            _context.Table.Remove(table);
            _context.SaveChanges();
        }

        public void UpdateTable(Table table) //Update a table
        {
            _context.Table.Update(table);
            _context.SaveChanges();
        }
    }
}
