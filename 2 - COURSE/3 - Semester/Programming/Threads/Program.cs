using System.Data;

namespace Threads
{
    internal class Program
    {

        #region thread_lock

        //private static int sharedCounter = 0;
        //private static readonly object lockObject = new object();

        //static void Main(string[] args)
        //{
        //    Thread myThread1 = new Thread(PrintNumbers);
        //    //myThread1.Start();

        //    Thread myThread2 = new Thread(PrintLetters);
        //    //myThread2.Start();

        //    Thread lockerThread = new Thread(IncrementCounter);
        //    Thread lockerThread2 = new Thread(IncrementCounter);

        //    lockerThread.Start();
        //    lockerThread2.Start();

        //    //myThread2.Join();
        //    //myThread1.Join();
        //    Console.WriteLine("End!");

        //}

        //static void PrintNumbers()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        Console.WriteLine(i.ToString());
        //        Thread.Sleep(1000);
        //    }
        //}

        //static void PrintLetters()
        //{
        //    for(char c = 'A';c <= 'E'; c++)
        //    {
        //        Console.WriteLine($"Letter {c}");
        //        Thread.Sleep(1000);
        //    }
        //}

        //static void IncrementCounter()
        //{
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        lock (lockObject)
        //        {
        //            sharedCounter++;
        //            Console.WriteLine(sharedCounter);
        //            Thread.Sleep(500);
        //        }
        //    }
        //}

        #endregion

        #region ThreadPool

        //static void Main()

        //{

        //    ThreadPool.QueueUserWorkItem(DoWork,"Task 1");
        //    ThreadPool.QueueUserWorkItem(DoWork, "Task2");

        //    Thread.Sleep(1000); // Give time for thread pool tasks to complete

        //}

        //static void DoWork(object state)

        //{

        //    Console.WriteLine($"{state} is being processed on thread {Thread.CurrentThread.ManagedThreadId}");

        //}

        #endregion

        #region Tasks

        static void Main(string[] args)
        {
            Thread evenThread = new Thread(PrintEvenNumber);
            Thread oddThread = new Thread(PrintOddNumber);

            //oddThread.Start();
            //evenThread.Start();

            //oddThread.Join();
            //evenThread.Join();

            ThreadPool.QueueUserWorkItem(DoSomeWork, "Task 1");
            ThreadPool.QueueUserWorkItem(DoSomeWork, "Task 2");
            ThreadPool.QueueUserWorkItem(DoSomeWork, "Task 3");

            Thread.Sleep(3000);
            Console.WriteLine("Main Thread End!");


        }

        static void PrintEvenNumber()
        {
            for (int i = 2; i <= 10; i+=2)
            {
                Console.WriteLine($"{i} - even number");
            }
        }

        static void PrintOddNumber()
        {
            for (int i = 1; i <= 10; i += 2)
            {
                Console.WriteLine($"{i} - odd number");
            }
        }

        static void DoSomeWork(object state)
        {
            Console.WriteLine($"{state} is being processed on thread {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine(Thread.CurrentThread.Name);
            Thread.Sleep(1000);
        }

        #endregion
    }
}

