namespace File_Processing_Simulation
{
    internal class Program
    {
        static private readonly string[] file_list = ["file1.txt", "file2.txt", "file3.txt", "file4.txt", "file5.txt"];

        static void Main(string[] args)
        {


            Thread fileReaderThread = new Thread(ReadFile);

            fileReaderThread.Start();
            fileReaderThread.Join();

            Thread.Sleep(10000);

            Console.WriteLine("Main Thread end!");

        }

        static void ReadFile()
        {

            foreach (var file in file_list)
            {
                string currentFile = file;
                Console.WriteLine($"Reading files: File Name: {currentFile}");
                ThreadPool.QueueUserWorkItem( _ => ProcessingFile(currentFile?.ToString() ?? "Null File"));     
                Thread.Sleep(1000);
            }
        }

        static void ProcessingFile(string name)
        {
            for (int i = 0; i <= 100; i+=10)
            {
                Console.WriteLine($"Processing file {name} .... [{i}%]");
                Thread.Sleep(500);                
            }
        }
    }
}
