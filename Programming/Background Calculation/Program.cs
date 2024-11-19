namespace Background_Calculation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => { Calcualte(); });
            Console.WriteLine("Background started!");
        }

        static void Calcualte()
        {
            for (int i = 0; i < 1000000; i++)
            {
                Console.WriteLine(i);
            }
        }
    }
}
