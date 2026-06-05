using System.Security.Cryptography.X509Certificates;

namespace Delegates
{
    public delegate int Hisoblash(int x, int y);
 
    internal class Program
    {
        static void Main(string[] args)
        {
            Hisoblash hisoblagich;
            hisoblagich = Qoshish;


            Func<int, int, int> adder = Ayirish;

            Console.WriteLine(adder(5,5));

            Nullable<int> n = null;


            Console.WriteLine(n);
        }

        public static int Qoshish(int a, int b)
        {
            return a + b;
        }

        public static int Ayirish(int a, int b)
        {
            return a - b;
        }
    }

    public abstract class MyAbstract
    {
        public abstract int MyProperty { get; set; }
    }

    public abstract class Animal
    {
        // Abstract property
        public abstract string Name { get; set; }

        // Abstract method
        public abstract void MakeSound();

        // Non-abstract method (optional in abstract classes)
        public void DisplayInfo()
        {
            Console.WriteLine($"I am an animal, and my name is {Name}.");
        }
    }

    interface Iinterface
    {
        public string Name { get; set; }
    }

}
