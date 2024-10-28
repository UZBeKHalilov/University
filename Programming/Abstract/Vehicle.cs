using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstract
{
    public abstract class Vehicle
    {
        public abstract void Info();
        public abstract void StartEngine();
    }

    public class Car : Vehicle
    {   
        public override void Info()
        {
            Console.WriteLine("Car info!");
        }
        public override void StartEngine()
        {
            Console.WriteLine("Car started!");
            
        }
    }

    public class Motorbike : Vehicle
    {
        public override void Info()
        {
            Console.WriteLine("Motorbike info!");
        }
        public override void StartEngine()
        {
            Console.WriteLine("Motorbike started!");
        }
    }
}
