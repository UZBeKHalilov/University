using System;
using System.Threading.Tasks;

namespace Fetch_Weather_Data
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Fetching weather data...");

            await FetchWeatherDataAsync();

            Console.WriteLine("Weather data retrieval complete.");
        }

        static async Task FetchWeatherDataAsync()
        {
            Console.WriteLine("Contacting weather service...");
            await Task.Delay(3000);

            string weatherDetails = "Weather: Sunny, Temperature: 25°C, Humidity: 60%";

            int raqamlar = 56;

            Console.WriteLine(raqamlar);

            Console.WriteLine(weatherDetails);
        }
    }
}
