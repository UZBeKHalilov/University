using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    using System;

    interface IFirst
    {
        void Display();
    }

    interface ISecond
    {
        void Display();
    }

    class MyClass : IFirst, ISecond
    {
        // IFirst interfeysining Display metodini aniq amalga oshiramiz
        void IFirst.Display()
        {
            Console.WriteLine("Display method from IFirst interface");
        }

        // ISecond interfeysining Display metodini aniq amalga oshiramiz
        void ISecond.Display()
        {
            Console.WriteLine("Display method from ISecond interface");
        }

        // MyClass uchun umumiy Display metod
        public void Display()
        {
            Console.WriteLine("General Display method from MyClass");
        }
    }

    

}
