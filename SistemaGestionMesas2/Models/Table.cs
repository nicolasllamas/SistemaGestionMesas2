using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionMesas2.Models
{
    public class Table
    {
        // Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public List<TableProduct> TableProducts { get; set; } = new List<TableProduct>();

        // Constructors
        public Table() { }

        public override string ToString()
        {
            return TableProducts != null && TableProducts.Any()
                ? $"La Mesa {Id} no está disponible"
                : $"La Mesa {Id} está disponible";
        }
    }
}
