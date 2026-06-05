using System;
using System.Threading.Tasks;

namespace Generate_Report_in_Background
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting report generation...");

            await GenerateReportAsync();

            Console.WriteLine("Report generation process complete.");
        }

        static async Task GenerateReportAsync()
        {
            Console.WriteLine("Generating report...");
            await Task.Delay(5000);
            Console.WriteLine("Report generated.");
        }
    }
}
