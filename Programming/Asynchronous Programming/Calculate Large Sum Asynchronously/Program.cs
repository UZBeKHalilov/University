namespace Calculate_Large_Sum_Asynchronously
{
    internal class Program
    {

        // ○ Implement a task that calculates the sum of a large range of numbers and allows the main program to continue
        // executing.


        static void Main(string[] args)
        {
            Task.Run(Calculating);

            for (int i = 0; i < 15; i++)
            {
                Console.WriteLine("Main task doing smth...");
                Thread.Sleep(1000);
            }

           
        }

        static void Calculating()
        {
            for (Int64 i = 0; i < 10000; i++)
            {
                Console.WriteLine(i*i);
                Thread.Sleep(50);
            }
        }

    }
}
