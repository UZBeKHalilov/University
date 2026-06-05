using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstract
{
    public abstract class Employee
    {
        public abstract double CalculateSalary();
    }
    public class FullTimeEmployee : Employee
    {
        public override double CalculateSalary()
        {
            return 2000;
        }
    }
    public class PartTimeEmployee : Employee
    {
        public override double CalculateSalary()
        {
            return 1250;
        }
    }
}
