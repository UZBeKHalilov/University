namespace Background_Image_Processing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread imgThjread = new Thread(ProcessingImage);

            imgThjread.Start();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main Function doing smth...");

                Thread.Sleep(1000);
            }
        }

        static void ProcessingImage()
        {

            for (int i = 0; i <= 10; i++)
            {
                Console.WriteLine($"Processing image...   [{i*10}%]");
                Thread.Sleep(i * 200);
            }

            Console.WriteLine("Image processed");
        }
    }
}
