using System;

using System.Net.Http;

using System.Threading.Tasks;

namespace async_await
{
    internal class Program
    {
        #region Learning
        //static async Task Main(string[] args)
        //{
        //    string url = "https://jsonplaceholder.typicode.com/posts/1";

        //    string result = await FetchDataAsync(url);

        //    Console.WriteLine(result);

        //    await Task.Delay(2000);

        //    Console.WriteLine("Task completed without deadlock");

        //}

        //static async Task<string> FetchDataAsync(string url)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        string response = await client.GetStringAsync(url);
        //        return response;
        //    }
        //}

        #endregion

        #region Task

        static async Task Main()

        {
            string url = "https://jsonplaceholder.typicode.com/posts/1";

            await FetchAndPrintDataAsync(url);

        }

        static async Task FetchAndPrintDataAsync(string url)

        {

            using (HttpClient client = new HttpClient())

            {
                string response = await client.GetStringAsync(url);

                Console.WriteLine(response.Substring(0, 50));
            }
        }

        #endregion
    }
}
