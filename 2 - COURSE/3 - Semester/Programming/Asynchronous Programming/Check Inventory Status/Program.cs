using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Check_Inventory_Status
{
    internal class Program
    {
        private static List<string> _Inventory = new List<string>() { "Olma", "Nok", "Uzum", "Kartoshka", "Piyoz" };

        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter the name of the product to check its inventory:");
            string productName = Console.ReadLine();

            string result = await ShowInventoryAsync(productName);
            Console.WriteLine(result);

            await Main(args);
        }

        public static async Task<string> ShowInventoryAsync(string productName)
        {
            Console.WriteLine($"Checking inventory for {productName}...");
            await Task.Delay(2000); 

            if (_Inventory.Contains(productName, StringComparer.OrdinalIgnoreCase))
            {
                return $"{productName} is in stock.";
            }
            else
            {
                return $"{productName} is not in stock.";
            }
        }
    }
}
