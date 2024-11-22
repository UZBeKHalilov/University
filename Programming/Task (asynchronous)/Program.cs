using System.Threading.Tasks;

namespace Task__asynchronous_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Task DoworkTask = Task.Run(DoWork);

            //DoworkTask.Wait();

            // ---------------------

            //Task.Run(() => DoWork())
            //    .ContinueWith(myPreviousTask => Console.WriteLine("Previous task completed!"))
            //    .Wait();

            //Task exTask = Task.Run(ThrowException);

            //try
            //{
            //    exTask.Wait();
            //}
            //catch (AggregateException ex)
            //{
            //    foreach (var innerException in ex.InnerExceptions)
            //    {
            //        Console.WriteLine($"Exception caught : {innerException.Message}");
            //    }
            //    throw;
            //}

            Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(i);
                    Thread.Sleep(500);
                }
            }).ContinueWith(t => Console.WriteLine("Task Completed!")).Wait();

                Task dangerTask = Task.Run(() => { ThrowException();});

            try
            {

                dangerTask.Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var item in ex.InnerExceptions)
                {
                    Console.WriteLine($"Caught exception: {item}");                    
                }
            }
                
            Console.WriteLine("Main Thread completed");
        }

        private static void ThrowException()
        {
            throw new InvalidOperationException("An error occurred in the task.");
        }
    

        static void DoWork()
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(i);
                }
            }
    }
}
