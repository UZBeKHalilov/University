using System;
using System.Threading;

namespace StockPriceMonitoring
{
    internal class Program
    {
        private static bool keepMonitoring = true;
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            Thread stockMonitorThread = new Thread(MonitorStockPrice);
            stockMonitorThread.Start();

            Console.WriteLine("Press any key to stop monitoring...");
            Console.ReadKey();

            keepMonitoring = false;

            stockMonitorThread.Join();

            Console.WriteLine("Stock price monitoring stopped.");
        }

        static void MonitorStockPrice()
        {
            double stockPrice = 100.0;
            while (keepMonitoring)
            {
                double change = Math.Round(random.NextDouble() * 2 - 1, 2);
                stockPrice = Math.Round(stockPrice + change, 2);

                Console.WriteLine($"Stock price: ${stockPrice} (Change: {change:+0.00;-0.00})");

                Thread.Sleep(1000);

            }
        }
    }
}
