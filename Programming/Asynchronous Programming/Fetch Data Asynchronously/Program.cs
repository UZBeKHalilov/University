namespace Fetch_Data_Asynchronously
{
    internal class Program
    {
        // ○ Create a task that simulates fetching data from an API by delaying for a few seconds and then printing “Data fetched.”
        static void Main(string[] args)
        {
            Task FetchData = Task.Run(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    Console.WriteLine("Fetching data from API...");
                    Thread.Sleep(1000/i);
                }

                Console.WriteLine("Data fetched");
            });

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main program doing some work...");
                Thread.Sleep(1000);
            }

            Console.WriteLine("Main program End!");
        }
    }
}
