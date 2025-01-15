using Microsoft.EntityFrameworkCore;
using SistemaGestionMesas2.Data;
using SistemaGestionMesas2.Helper;
using SistemaGestionMesas2.Models;
using SistemaGestionMesas2.Repository;

public static class TableMenuManager
{
    public static bool ManageTables()
    {
        bool isWorking = true;

        Console.Clear();
        Console.WriteLine("Gestion de Mesas\n");

        using (var context = new ApplicationDbContext())
        {
            var tableRepository = new TableRepository(context);
            var amountOfTables = tableRepository.GetAllTables().Count();
            Console.WriteLine($"La cantidad total de mesas es {amountOfTables}.");
            var amountOfTablesAvailable = tableRepository.GetAllTables()
                .Include(t => t.TableProducts)
                .Where(p => !p.TableProducts.Any())
                .Count();
            Console.WriteLine($"Las mesas disponibles son {amountOfTablesAvailable}.");
        }

        Console.WriteLine("\nIndique que desea hacer:");
        Console.WriteLine("1. Agregar productos a una mesa");
        Console.WriteLine("2. Consultar las mesas");
        Console.WriteLine("3. Modificar la cantidad de mesas");
        Console.WriteLine("4. Cerrar y liberar una mesa");
        Console.WriteLine("5. Salir");

        int option = InputHelper.GetValidInt(1, 5);

        switch (option)
        {
            case 1:
                while (AddProductToTable()) ;
                Console.WriteLine("\nPresione cualquier tecla para continuar.");
                Console.ReadKey();
                break;
            case 2:
                GetTablesOverview();
                Console.WriteLine("\nPresione cualquier tecla para continuar.");
                Console.ReadKey();
                break;
            case 3:
                ChangeAmountTable();
                Console.WriteLine("\nPresione cualquier tecla para continuar.");
                Console.ReadKey();
                break;
            case 4:
                // THIS ONE IS REMAINING TO BE IMPLEMENTED

                Console.WriteLine("\nPresione cualquier tecla para continuar.");
                Console.ReadKey();
                break;

            case 5:
                isWorking = false;
                break;

            default:
                Console.WriteLine("Opción inválida");
                break;
        }

        return isWorking;
    }
    public static bool AddProductToTable()
    {
        bool continueWorking = true;
        Console.Write("Indique el ID de la mesa: ");
        int selectedTable = SelectingTable();

        Table table = null;
        TableRepository tableRepository = null;
        using (var context = new ApplicationDbContext())
        {
            tableRepository = new TableRepository(context);
            table = tableRepository.GetTableByIdWithProducts(selectedTable);
        }

        if (table == null)
        {
            Console.WriteLine("Mesa no encontrada.");
        }

        else
        {
            bool isAddingProduct = true;
            while (isAddingProduct) // While the user wants to keep adding products to the selected table
            {
                Console.WriteLine();
                ProductMenuManager.DisplayProductAndMenu();
                Console.Write("\nIndique el ID del producto que desea agregar: ");
                int selectedProduct = SelectingProduct();

                var existingTableProduct = table.TableProducts
                    .FirstOrDefault(tp => tp.ProductId == selectedProduct);

                if (existingTableProduct != null)
                {
                    // If the product is already on the table, update its quantity
                    Console.WriteLine("Este producto ya está en la mesa. ¿Desea aumentar la cantidad? (s/n)");
                    var increaseQuantity = Console.ReadLine()?.ToLower() == "s";

                    if (increaseQuantity)
                    {
                        Console.Write("Indique la cantidad que desea adicionar: ");
                        int additionalQuantity = InputHelper.GetValidInt(1);
                        existingTableProduct.Quantity += additionalQuantity;

                        using (var context = new ApplicationDbContext())
                        {
                            var tableProductRepository = new TableProductRepository(context);
                            tableProductRepository.ChangeQuantity(selectedTable, selectedProduct, existingTableProduct.Quantity);
                        }
                    }
                }
                else
                {
                    // If the product is not on the table, create a new TableProduct entry
                    Console.Write("Indique la cantidad: ");
                    int quantity = InputHelper.GetValidInt(1);

                    var tableProduct = new TableProduct
                    {
                        TableId = selectedTable,
                        ProductId = selectedProduct,
                        Quantity = quantity
                    };

                    using (var context = new ApplicationDbContext())
                    {
                        var tableProductRepository = new TableProductRepository(context);
                        tableProductRepository.AddTableProduct(tableProduct); // Save the changes to the table
                    }
                    Console.WriteLine("Producto agregado a la mesa.");
                }
                Console.WriteLine("¿Desea agregar otro producto a la mesa? (s/n)");
                isAddingProduct = Console.ReadLine()?.ToLower() == "s";
            }
        }
        Console.WriteLine("¿Desea agregar un producto a otra mesa? (s/n)");
        return continueWorking = Console.ReadLine()?.ToLower() == "s";
    }

    public static void ChangeAmountTable()
    {
        Console.Write("Indique la cantidad de mesas:");
        int amount = InputHelper.GetValidInt(1);
        bool sameAmount;

        using (var context = new ApplicationDbContext())
        {
            var tableRepository = new TableRepository(context);
            var tables = tableRepository.GetAllTables().ToList();

            if (tables.Count < amount)
            {
                for (int i = tables.Count; i < amount; i++) { tableRepository.AddTable(); }
                sameAmount = false;
            }
            else if (tables.Count > amount)
            {
                for (int i = tables.Count; i > amount; i--) { tableRepository.RemoveTable(tables[i - 1]); }
                sameAmount = false;
            }
            else { sameAmount = true; }
        }
        string result = sameAmount
            ? "La cantidad de mesas indicada es igual a la actual."
            : "La cantidad de mesas ha sido modificada.";
        Console.WriteLine(result);

    }
    public static void GetTablesOverview()
    {
        //Display if the table is available or not
        //In the future, it should also say the products in the table

        using (var context = new ApplicationDbContext())
        {
            var tables = new TableRepository(context).GetAllTables().ToList();

            if (tables.Any())
            {
                foreach (var table in tables) { Console.WriteLine(table); }
            }
            else { Console.WriteLine("No hay mesas en la base de datos."); }
        }
    }
    public static int SelectingProduct()
    {
        int highestId;
        using (var context = new ApplicationDbContext())
        {
            var productRepository = new ProductRepository(context);
            highestId = productRepository.GetAllProducts().Max(t => t.Id);
        }
        return InputHelper.GetValidInt(1, highestId);
    }

    public static int SelectingTable()
    {
        int highestId;
        using (var context = new ApplicationDbContext())
        {
            var tableRepository = new TableRepository(context);
            highestId = tableRepository.GetAllTables().Max(t => t.Id);
        }
        return InputHelper.GetValidInt(1, highestId);
    }
}