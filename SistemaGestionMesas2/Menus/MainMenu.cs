using SistemaGestionMesas2;
using SistemaGestionMesas2.Data;
using SistemaGestionMesas2.Helper;
using SistemaGestionMesas2.Models;
using SistemaGestionMesas2.Repository;

internal static class MainMenu
{

    public static bool Display()
    {
        bool isWorking = true;
        Console.Clear();
        Console.WriteLine("Bienvenido al Sistema de Gestion de Mesas\n");
        Console.WriteLine("Indique que desea hacer:");
        Console.WriteLine("1. Gestionar las mesas");
        Console.WriteLine("2. Gestionar productos y menus");
        Console.WriteLine("3. Salir");

        int option = InputHelper.GetValidInt(1, 3);
        switch (option)
        {
            case 1:
                while (TableMenuManager.ManageTables()) ;
                break;
            case 2:
                while (ProductMenuManager.ManageProduct()) ;
                break;
            case 3:
                Console.WriteLine("Saliendo de la aplicación.");
                isWorking = false;
                break;
            default:
                Console.WriteLine("Opción inválida");
                break;
        }

        return isWorking;
    }
}