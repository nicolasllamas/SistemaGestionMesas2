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
        Console.WriteLine("3. Cerrar y liberar una mesa");
        Console.WriteLine("4. Modificar la cantidad de mesas");
        Console.WriteLine("5. Volver al menu anterior");

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
                FinalizeTable();
                Console.WriteLine("\nPresione cualquier tecla para continuar.");
                Console.ReadKey();
                break;

            case 4:
                ChangeAmountTable();
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
        using (var context = new ApplicationDbContext())
        {
            var tables = new TableRepository(context).GetAllTables().ToList();
            var allProduct = new ProductRepository(context).GetAllProducts().ToList();
            
            Console.WriteLine();
            if (tables.Any())
            {
                foreach (var table in tables)
                {
                    if (table.TableProducts.Any())
                    {
                        Console.WriteLine($"La mesa {table.Id} posee los siguientes productos:");
                        foreach (TableProduct tableProduct in table.TableProducts)
                        {
                            Console.WriteLine($"Producto: {tableProduct.Product.Name}, Cantidad: {tableProduct.Quantity}, Precio por unidad: ${allProduct.Find(x => x.Id == tableProduct.ProductId).Price:F2}");
                        }
                    }
                    else { Console.WriteLine(table); }
                    Console.WriteLine();
                }
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

    public static void FinalizeTable()
    {
        Console.Write("Indique el ID de la mesa: ");
        int selectedTable = SelectingTable();

        Table table = null;
        TableRepository tableRepository = null;
        List<Product> allProduct = null;
        using (var context = new ApplicationDbContext())
        {
            tableRepository = new TableRepository(context);
            table = tableRepository.GetTableByIdWithProducts(selectedTable);
            allProduct = new ProductRepository(context).GetAllProducts().ToList();
        }
        decimal amountToPay = 0;

        if (table == null)
        {
            Console.WriteLine("Mesa no encontrada.");
        }
        else
        {
            Console.WriteLine($"La mesa {selectedTable} compró los siguientes productos:");

            foreach (TableProduct tableProduct in table.TableProducts)
            {
                Console.WriteLine($"Producto: {tableProduct.Product.Name}, Cantidad: {tableProduct.Quantity}, Precio por unidad: ${allProduct.Find(x => x.Id == tableProduct.ProductId).Price:F2}");
                amountToPay += tableProduct.Quantity * allProduct.Find(x => x.Id == tableProduct.ProductId).Price;
            }
            Console.WriteLine("\nEl total a pagar es: $" + amountToPay);
            Console.WriteLine("¿Está seguro que desea liberar esta mesa?");
            Console.WriteLine("1. Sí");
            Console.WriteLine("2. No");
            int option = InputHelper.GetValidInt(1, 2);

            if (option == 1)
            {
                using (var context = new ApplicationDbContext())
                {
                    var tableProductRepository = new TableProductRepository(context);
                    tableProductRepository.RemoveAllProductsFromTable(selectedTable);
                }
                Console.WriteLine("La mesa ha sido liberada.");
            }
            else { Console.WriteLine("Operación cancelada."); }
        }
    }
}