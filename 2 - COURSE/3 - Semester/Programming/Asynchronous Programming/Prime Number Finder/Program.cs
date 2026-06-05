using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Prime_Number_Finder
{
    internal class Program
    {
        // Periodic Message Display:
        // ○ Start a thread that displays a reminder message to the user every 10 seconds while the main program runs other tasks.
        static void Main(string[] args)
        {

            Thread RemindingThread = new Thread(_ => FindPrimeNumber(10, 2000));
            RemindingThread.Start();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main program doing other task...");
                Thread.Sleep(1000);
            };

            RemindingThread.Join();
        }

        static void FindPrimeNumber(int StartWith, int EndWith)
        {

            for (int i = StartWith; i <= EndWith; i++)
            {
                if (IsPrime(i))
                    Console.WriteLine($"{i} Prime number.");

                Thread.Sleep(500);

            }

            Console.WriteLine("End!");
        }

        static bool IsPrime(int number)
        {

            if (number <= 1)
                return false;

            for (int i = 2; i <= Math.Sqrt(number); i++) // Checking only to numnber's sqrt
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }
    }
}
