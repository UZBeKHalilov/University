using System;
using System.Threading.Tasks;

namespace Payment_Processing_Simulation
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting payment process...");

            await ProcessPaymentAsync();

            Console.WriteLine("Payment process complete.");
        }

        static async Task ProcessPaymentAsync()
        {
            Console.WriteLine("Processing payment...");
            await Task.Delay(3000);
            Console.WriteLine("Payment successful.");
        }
    }
}
