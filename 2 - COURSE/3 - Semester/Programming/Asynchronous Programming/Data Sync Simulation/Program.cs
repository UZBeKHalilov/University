using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics.X86;
using System.Threading;

namespace Data_Sync_Simulation
{
    internal class Program
    {

        //  Data Sync Simulation:
        // ○ Use a thread to simulate syncing data between a device and a server every few seconds, printing a “Syncing data…” message.

        static void Main(string[] args)
        {
            Thread syncThread = new Thread(Syncing);

            Console.WriteLine("Starting syncing data between device and server:\n");
            syncThread.Start();
        }

        static void Syncing()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{DateTime.Now} Syncing data...");
                Thread.Sleep(1000);
            }

            Console.WriteLine("Syncing data end!");
        }
    }
}
