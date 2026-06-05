using System;
using System.Threading.Tasks;

namespace Email_Sending_Simulation
{
    internal class Program
    {
        // ○ Use a task to simulate sending an email. Delay the task to mimic processing time and then print “Email sent.”
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting email sending simulation...");

            await SendEmailAsync("Hello, this is a test email!");

            Console.WriteLine("Simulation complete.");
        }

        static async Task SendEmailAsync(string text)
        {
            Console.WriteLine("Processing email...");
            await Task.Delay(1000); 
            Console.WriteLine("Email sent!");
        }
    }
}
