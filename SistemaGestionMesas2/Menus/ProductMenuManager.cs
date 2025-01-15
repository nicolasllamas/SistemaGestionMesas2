using SistemaGestionMesas2.Data;
using SistemaGestionMesas2.Helper;
using SistemaGestionMesas2.Models;
using SistemaGestionMesas2.Repository;

internal static class ProductMenuManager
{
    public static void DeleteProductOrMenu()
    {
        Console.Write("Indique el ID del producto:");
        int selection = InputHelper.GetValidInt(1);

        using (var context = new ApplicationDbContext())
        {
            var productRepository = new ProductRepository(context);
            var product = new ProductRepository(context).GetProductById(selection);

            if (product != null)
            {
                Console.WriteLine($"Producto seleccionado: {product.Name}");
                Console.WriteLine("¿Está seguro que desea eliminar este producto?");
                Console.WriteLine("1. Sí");
                Console.WriteLine("2. No");
                int option = InputHelper.GetValidInt(1, 2);

                if (option == 1)
                {
                    productRepository.RemoveProduct(product);
                    Console.WriteLine("Producto eliminado.");
                }
                else { Console.WriteLine("Operación cancelada."); }
            }
            else { Console.WriteLine("Producto no encontrado."); }
        }
    }
    public static void DisplayProductAndMenu()
    {
        using (var context = new ApplicationDbContext())
        {
            var allProduct = new ProductRepository(context).GetAllProducts().ToList();

            if (allProduct.Any())
            {
                Console.WriteLine("Productos en la base de datos:");
                foreach (var product in allProduct)
                {
                    Console.WriteLine($"ID: {product.Id}, Nombre: {product.Name}, Precio: ${product.Price:F2}");
                }
            }
            else
            {
                Console.WriteLine("No hay productos en la base de datos.");
            }

        }
    }
    public static void ManageAddingMenu()
    {
        Console.Write("Indique el nombre:");
        string nameNewMenu = null;
        while (nameNewMenu == null) { nameNewMenu = Console.ReadLine(); }

        Console.Write("Indique el precio:");
        decimal priceNewMenu = InputHelper.GetPositiveDecimal();

        Console.Write("Indique el tiempo de cocción (min):");
        int timeCooking = InputHelper.GetValidInt(1);

        using (var context = new ApplicationDbContext())
        {
            var menuRepository = new MenuRepository(context);
            menuRepository.AddMenu(nameNewMenu, priceNewMenu, timeCooking);
        }
        Console.Write("El menú ha sido agregado.");
    }
    public static void ManageAddingProduct()
    {
        Console.Write("Indique el nombre:");
        string nameNewProduct = Console.ReadLine();
        Console.Write("Indique el precio:");
        decimal priceNewProduct = InputHelper.GetPositiveDecimal();

        using (var context = new ApplicationDbContext())
        {
            var productRepository = new ProductRepository(context);
            productRepository.AddProduct(nameNewProduct, priceNewProduct);
        }

        Console.Write("El producto ha sido agregado.");
    }

    public static bool ManageProduct()
    {
        bool isWorking = true;

        Console.Clear();
        Console.WriteLine("Gestion de Productos\n");

        Console.WriteLine("Indique que desea hacer:");
        Console.WriteLine("1. Ver lista de menu y productos");
        Console.WriteLine("2. Agregar un nuevo producto");
        Console.WriteLine("3. Agregar un nuevo menu");
        Console.WriteLine("4. Modificar un producto o menu existente");
        Console.WriteLine("5. Eliminar un producto o menu");
        Console.WriteLine("6. Volver al menu anterior");

        int option = InputHelper.GetValidInt(1, 6);

        switch (option)
        {
            case 1:
                DisplayProductAndMenu();
                Console.WriteLine("\nPresione cualquier tecla para continuar.");
                Console.ReadKey();
                break;
            case 2:
                ManageAddingProduct();
                Console.WriteLine("\nPresione cualquier tecla para continuar.");
                Console.ReadKey();
                break;
            case 3:
                ManageAddingMenu();
                Console.WriteLine("\nPresione cualquier tecla para continuar.");
                Console.ReadKey();
                break;
            case 4:
                ModifyProductOrMenu();
                Console.WriteLine("\nPresione cualquier tecla para continuar.");
                Console.ReadKey();
                break;
            case 5:
                DeleteProductOrMenu();
                Console.WriteLine("\nPresione cualquier tecla para continuar.");
                Console.ReadKey();
                break;
            case 6:
                isWorking = false;
                break;
            default:
                Console.WriteLine("Opción inválida");
                break;
        }

        return isWorking;
    }
    public static void ModifyProductOrMenu()
    {
        Console.Write("Indique el ID del producto:");
        int selection = InputHelper.GetValidInt(1);

        using (var context = new ApplicationDbContext())
        {
            var productRepository = new ProductRepository(context);
            var product = new ProductRepository(context).GetProductById(selection);

            if (product != null)
            {
                int amountOfOptions = 2;
                Console.WriteLine($"Producto seleccionado: {product.Name}");
                Console.WriteLine("Indique que desea modificar:");
                Console.WriteLine("1. Nombre");
                Console.WriteLine("2. Precio");
                if (product is Menu)
                {
                    Console.WriteLine("3. Tiempo de cocción");
                    amountOfOptions = 3;
                }
                int option = InputHelper.GetValidInt(1, amountOfOptions);
                switch (option)
                {
                    case 1:
                        Console.Write("Indique el nuevo nombre:");
                        product.Name = Console.ReadLine();
                        break;
                    case 2:
                        Console.Write("Indique el nuevo precio:");
                        product.Price = InputHelper.GetPositiveDecimal();
                        break;
                    case 3:
                        if (product is Menu menu)
                        {
                            Console.Write("Indique el nuevo tiempo de cocción:");
                            menu.TimeToCook = InputHelper.GetValidInt(1);
                        }
                        break;
                    default:
                        Console.WriteLine("Opción inválida");
                        break;
                }
                productRepository.UpdateProduct(product);
                Console.WriteLine("Producto modificado.");
            }
            else { Console.WriteLine("Producto no encontrado."); }
        }

    }
}