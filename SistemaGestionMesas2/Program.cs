using SistemaGestionMesas2.Data;
using SistemaGestionMesas2.Helper;
using SistemaGestionMesas2.Models;
using SistemaGestionMesas2.Repository;

public class Program
{
    public static void Main(string[] args)
    {
        bool isWorking = true;
        while (isWorking) { isWorking = MainMenu.Display(); }
        Environment.Exit(0);
    }
}
