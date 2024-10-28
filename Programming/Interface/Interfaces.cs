using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IVehicle
    {
        void StartEngine();
        void StopEngine();
    }

    public interface IAnimal
    {
        void MakeSound();
    }
    
    public interface IFlyable : IAnimal
    {
        public void Fly();
    }

    public interface IShape
    {
        double GetArea();
    }

    public interface IReadable
    {
        void Read();
    }

    public interface IWriteable
    {
        void Write(string text);
    }

    public interface IPerson
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
    }

    public interface ICalculator
    {
        int Calculate(int a, int b);
    }

    public interface IDriveable
    {
        void Drive();
    }

    public interface IFlyableCar
    {
        void Fly();
    }

    public interface IComparableItem
    {
        int CompareTo(IComparableItem other);
    }

    public interface IClickable
    {
        void Click();
    }

    public interface IFileOperation
    {
        void ReadFile(string filePath);
        void WriteFile(string filePath, string content);
    }
}
