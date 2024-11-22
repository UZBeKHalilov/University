using System.Threading.Tasks;
using System.Threading;

namespace Periodic_Message_Display
{
    internal class Program
    {
        // Periodic Message Display:
        // ○ Start a thread that displays a reminder message to the user every 10 seconds while the main program runs other tasks.
        static void Main(string[] args)
        {

            Thread RemindingThread = new Thread(RemindingMessage);
            RemindingThread.Start();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main program doing other task...");
                Thread.Sleep(1000);
            };

            RemindingThread.Join();
        }

        static void RemindingMessage()
        {
            for (int i = 15; i > 0; i--)
            {

                Console.WriteLine($"{i} Seconds remaining!!");
                Thread.Sleep(1000);
            }

            Console.WriteLine("End!");
        }
    }
}
