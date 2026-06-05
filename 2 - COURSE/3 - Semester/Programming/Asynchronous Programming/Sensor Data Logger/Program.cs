namespace Sensor_Data_Logger
{
    internal class Program
    {

        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            // Use a thread to simulate a device sensor that logs random temperature data to the console every few seconds.

            Thread tempThread = new Thread(SensorDataLogger);

            tempThread.Start();

            tempThread.Join();

            Console.WriteLine("Main thread End!");

        }

        static void SensorDataLogger()
        {

            for (int i = 0; i < 10; i++)
            {
                double temperature = Math.Round(random.NextDouble() * 50 - 10, 1);

                Console.WriteLine($"Temperature: {temperature}°C");

                Thread.Sleep(random.Next(1000, 3000));
            }
            
        }    
    }
}
