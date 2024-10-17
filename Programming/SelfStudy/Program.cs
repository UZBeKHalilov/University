using System.Xml.Serialization;

namespace SelfStudy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Programs");
            Choose();

        }

        static void Choose()
        {
            Console.WriteLine("Choose program: ");
            Console.WriteLine("1 - Search for number\n" +
                "2 - Find max number\n" +
                "3 - Find min number");

            switch (GetKey())
            {
                case ConsoleKey.D1:
                    Search();
                    break;
                case ConsoleKey.D2:
                    FindMaxNumber();
                    break;
                case ConsoleKey.D3:
                    FindMinNumber();
                    break;
                //case ConsoleKey.D4:
                //    break;
                //case ConsoleKey.D5:
                //    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Error!");
                    Choose();
                    break;
            }
            Console.WriteLine("\n\n\n\n");
            Choose();

        }

        static void Search()
        {
            
            int[] MyArrays = { 1, 2, 3, 4, 5, 13, 56};

            Console.WriteLine("Our numbers : ");
            foreach (var item in MyArrays)
            {
                Console.Write(item + ", ");
            }
            
            Console.WriteLine("Choose number to search:");
            string number = Console.ReadLine();

            Console.WriteLine($"Result: {IsThere(MyArrays, number)}");
        }
        static void FindMaxNumber() 
        {
            int[] MyArrays = new int[] { 1, 2, 3, 56, 123, 12 };
            int result = 0;

            foreach (var item in MyArrays)
            {
                Console.Write(item + " ,");
                result = item;
                foreach (var item1 in MyArrays)
                {
                    if (result > item1)
                        continue;
                    
                    result = item1;
                }
            }
            
            Console.WriteLine($"\nThe max number: {result}");
        }
        
        static void FindMinNumber()
        {
            int[] MyArrays = new int[] { 1, 2, 3, 56, 123, 12 };
            int result = 0;

            foreach (var item in MyArrays)
            {
                result = item;
                foreach (var item1 in MyArrays)
                {
                    if (result < item1)
                        continue;

                    result = item1;
                }
                Console.Write(item + " ,");
            }

            Console.WriteLine($"\nThe min number: {result}");
        }
        static bool IsThere(int[] array, string text)
        {
            int num = int.Parse(text);

            bool result = false;
            foreach (var item in array)
            {
                if(item == num)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        static ConsoleKey GetKey()
        {
            Console.Write("[]> ");
            ConsoleKey key = Console.ReadKey().Key;
            Console.WriteLine();
            return key;
        }
    }
}
