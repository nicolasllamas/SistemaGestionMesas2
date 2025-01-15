using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionMesas2.Models
{
    public class TableProduct
    {
        // Properties
        // Attributes were defined in ApplicationDbContext
        public int TableId { get; set; }

        public Table Table { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
