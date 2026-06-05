using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Runtime.ExceptionServices;
using System.Security.AccessControl;

namespace Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Calculator!");

            int firstNumber;
            int secondNumber;
            ConsoleKeyInfo operation;
            int result = 0;

            print("Enter First number:");

            firstNumber = int.Parse(Console.ReadLine());

            print("Choose operation: \n [1] +\n [2] -\n [3] *\n [4] /");

            operation = Console.ReadKey();

            print("\nEnter Second number: ");
            secondNumber = int.Parse(Console.ReadLine());

            Calculating(operation.Key, firstNumber, secondNumber);                       
        }

        static void print(String text)
        {
            Console.WriteLine(text);
            Console.Write("[]>");
        }

        // addition, subtraction, multiplication, division

        static void Calculating(ConsoleKey operation, int firstNumber, int secondNumber)
        {
            int result = 0;

            switch (operation)
            {
                case ConsoleKey.D1:
                    result = firstNumber + secondNumber;
                    break;

                case ConsoleKey.D2:
                    result = firstNumber - secondNumber;
                    break;

                case ConsoleKey.D3:
                    result = firstNumber * secondNumber;
                    break;

                case ConsoleKey.D4:
                    result = firstNumber / secondNumber;
                    break;

                default:
                    Console.Clear();
                    break;
            }

            Console.WriteLine($"\n = {result}");
        }
    }
}
