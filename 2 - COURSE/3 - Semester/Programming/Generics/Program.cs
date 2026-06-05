

namespace Generics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Generics!");

            GenericClass<int> intObj = new GenericClass<int>(5);

            GenericClass<string> stringObj = new GenericClass<string>("How are you?");

            Console.WriteLine(intObj.GetData());
            Console.WriteLine(stringObj.GetData());


        }

        static void Display<T>(T value)
        {
            Console.WriteLine(value);
        }
    }
}
