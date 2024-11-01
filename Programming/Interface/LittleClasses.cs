using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Interface
{
    public class Car : IVehicle
    {
        public void StartEngine()
        {
            Console.WriteLine("Car started!");
        }


        public void StopEngine()
        {
            Console.WriteLine("Car stopped!");
        }
    }

    public class Motorsicle : IVehicle
    {
        public void StartEngine()
        {
            Console.WriteLine("Bike started!");
        }

        public void StopEngine()
        {
            Console.WriteLine("Bike stopped!");
        }
    }

    //-------------------

    public class Bird : IAnimal, IFlyable
    {
        public void MakeSound()
        {
            Console.WriteLine("Chirp!");
        }
        public void Fly()
        {
            Console.WriteLine("Bird is flying!");
        }
    }

    //------------------- 

    public class Circle : IShape
    {
        private double Radius {get; set; }
        public Circle(double radius)
        {
            Radius = radius;
        }
        public double GetArea()
        {
            return Math.PI * Radius * Radius;
        }
    }

    public class Ractangle : IShape
    {
        private double Length { get; set; }
        private double Width { get; set; }

        public Ractangle(double length, double width)
        {
            Length = length;
            Width = width;
        }

        public double GetArea()
        {
            return Length * Width;
        }
    }
    
    //------------------- 

    public class Document : IReadable,IWriteable
    {
        private string Text {get; set;}

        public Document(string text = "Empty")
        {
            Text = text;
        }


        public void Read()
        {
            Console.WriteLine($"Reading: {Text}");
        }

        public void Write(string text)
        {
            Text = text;
            Console.WriteLine($"Writing: {text}");
        }
        
    }
    
    //------------------- 

    public class Student : IPerson
    {
        public string Name {get; set;}
        public int Age { get; set; }

        public Student(string name = "No Name", int age = 0)
        {
            Name = name;
            Age = age;
        }
        public void Print()
        {
            Console.WriteLine($"{Name} is a student.");
            Console.WriteLine($"{Name} is {Age} years old.");
        }
    }
    
    //------------------- 

    class Addition : ICalculator
    {
        public int Calculate(int a, int b)
        {
            return a + b;
        }
    }

    
    public class Subtraction : ICalculator
    {
        private int _A { get; set; }
        private int _B { get; set; }
    
        public int Calculate(int a, int b)
        {
            return a - b;
        }
    }
    
    
    //------------------- 

    public class FlyingCar : IDriveable, IFlyableCar
    {
        public void Fly()
        {
            Console.WriteLine("Car started fly!");
        }

        public void Drive()
        {
            Console.WriteLine("Car started drive!");
        }
    }

    //------------------- 

    public class Product : IComparableItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        // Implement the CompareTo method to compare products based on their price
        public int CompareTo(IComparableItem other)
        {
            if (other is Product otherProduct)
            {
                return Price.CompareTo(otherProduct.Price);
            }
            throw new ArgumentException("Object is not a Product");
        }

        // To display Product information
        public override string ToString()
        {
            return $"{Name}: ${Price:F2}";
        }
    }

    //------------------- 

    public class Button : IClickable
    {
        private int x = 1;
        private int y = 2;
        public void Click()
        {
            Console.WriteLine($"Button has been clicked!{x} and {y}");
        }
    }

    public class Hyperlink : IClickable
    {
        private int x = 0;
        private int y = 0;
        
        public void Click()
        {
            Console.WriteLine($"Hyperlink has been clicked! {x} and {y}");
        }
    }

    //------------------- 

    public class TextFileOperation : IFileOperation
    {
        // Method to read content from a file
        public void ReadFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath);
                    Console.WriteLine("File Content:");
                    Console.WriteLine(content);
                }
                else
                {
                    Console.WriteLine("File does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }
        }

        // Method to write content to a file
        public void WriteFile(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
                Console.WriteLine("Content successfully written to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }
    }
}