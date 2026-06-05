using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstract
{
    public abstract class Animals
    {
        public abstract void MakeSound();
    }

    public class Dog : Animals
    {
        public override void MakeSound()
        {
            Console.WriteLine("Woof!");
        }
    }
    public class Cat : Animals
    {
        public override void MakeSound()
        {
            Console.WriteLine("Meow!");
        }
    }
}
