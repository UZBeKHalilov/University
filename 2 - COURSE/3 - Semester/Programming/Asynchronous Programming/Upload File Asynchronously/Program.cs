using System;
using System.Threading.Tasks;

namespace Upload_File_Asynchronously
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting file upload simulation...");

            await UploadFileAsync();

            Console.WriteLine("File upload simulation complete.");
        }

        static async Task UploadFileAsync()
        {
            Console.WriteLine("Uploading file...");
            await Task.Delay(3000); 
            Console.WriteLine("File uploaded.");
        }
    }
}
