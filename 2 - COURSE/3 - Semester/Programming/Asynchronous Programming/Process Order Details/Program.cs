namespace Process_Order_Details
{
    internal class Program
    {

        // Use a task to simulate processing an order with a delay, then return an “Order processed” message.

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Processing order...");

            string result = await ProcessOrderAsync();
            string result2 = await ProcessOrderAsync();
            Console.WriteLine(result + ' ' + result2);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main thread doing some work");
                Thread.Sleep(100*i);
            }
        }

        public static async Task<string> ProcessOrderAsync()
        {
            await Task.Delay(1000);

            return "Order processed";
        }
    }
}
