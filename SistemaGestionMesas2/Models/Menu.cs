    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace SistemaGestionMesas2.Models
    {
        public class Menu : Product
        {
            public int TimeToCook { get; set; } // time to cook in minutes

            public Menu() : base() { }

            public Menu(string name, decimal price, int timeToCook)
                : base(name, price)
            {
                TimeToCook = timeToCook;
            }

            public override string ToString()
            {
                return base.ToString() + $", cocción: {TimeToCook} minutos";
            }
        }
    }
