namespace SistemaGestionMesas2.Helper
{
    public class InputHelper
    {
        public static int GetValidInt(int min, int max) // min and max are inclusive values
        {
            int result = default;

            while (!int.TryParse(Console.ReadLine(), out result) || result < min || result > max)
            {
                Console.WriteLine($"Indique un valor valido (minimo {min}, máximo {max})");
            }
            return result;
        }
        public static int GetValidInt(int min) // OVERLOAD, min is a inclusive value
        {
            int result = default;

            while (!int.TryParse(Console.ReadLine(), out result) || result < min || result > int.MaxValue)
            {
                Console.WriteLine($"Indique un valor valido (minimo {min}, máximo {int.MaxValue})");
            }
            return result;
        }

        public static decimal GetPositiveDecimal() // min is < 0
        {
            decimal result = default;
            while (!decimal.TryParse(Console.ReadLine(), out result) || result <= 0)
            {
                Console.WriteLine("Indique un valor valido (mayor a 0)");
            }
            return result;
        }
    }
}
