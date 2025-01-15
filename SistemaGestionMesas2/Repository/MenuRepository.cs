using SistemaGestionMesas2.Data;
using SistemaGestionMesas2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionMesas2.Repository
{
    public  class MenuRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddMenu(string name, decimal price, int timeToCook) // Add a menu to the database
        {
            Menu menu = new Menu(name, price, timeToCook);
            _context.Product.Add(menu);
            _context.SaveChanges();
        }

        public void AddMenu(Menu menu) //Overload, not used in the project. Might be useful in the future
        {
            _context.Product.Add(menu);
            _context.SaveChanges();
        }

    }
}
