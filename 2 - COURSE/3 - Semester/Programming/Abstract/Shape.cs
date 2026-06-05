using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstract
{
    public abstract class Shape
    {
        public abstract void CalculateArea();
    }
    
    public class Rectangle : Shape
    {
        public double Length { get; set; }
        public double Width { get; set; }

        public Rectangle(double length, double width)
        {
            Length = length;
            Width = width;
        }

        public override void CalculateArea()
        {
            Console.WriteLine("Area of the rectangle: " + (Length * Width));
        }
    }
    public class Circle : Shape
    {
        public double Radius { get; set; }
        public Circle(double radius)
        {
            Radius = radius;
        }

        public override void CalculateArea()
        {
            Console.WriteLine("Area of the circle: " + (Math.PI * Radius * Radius));
        }
    }
}
