//Task 6: Employee Payroll System

//Description: Create a payroll system that calculates the salary of multiple employees
//based on their hourly rate and hours worked. Use an array to store employee details and a do-while loop to compute payroll for each employee.

//    Requirements:

//Store employee names, hourly rates, and hours worked in arrays.
//    Calculate and display the salary for each employee.
//    Handle multiple employees in the system.

// -------------------------------------------------------------

//Задача 6: Система расчета заработной платы сотрудников

//Описание: Создайте систему расчета заработной платы, которая рассчитывает заработную плату нескольких сотрудников на основе их почасовой ставки
//и отработанных часов. Используйте массив для хранения данных о сотрудниках и цикл do-while для расчета заработной платы для каждого сотрудника.

//    Требования:

//Сохраняйте имена сотрудников, почасовые ставки и отработанные часы в массивах.
//    Рассчитывайте и отображайте заработную плату для каждого сотрудника.
//    Обрабатывайте нескольких сотрудников в системе.

namespace EmployeePayrollSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {

            SalaryCalculation();
        }

        static void SalaryCalculation()
        {
            #region Employee Data Initialization

            string[] employeeNames = { "Abdulloh", "Ortiq", "Mohir", "Abduqodir" };
            double[] hourlyRates = { 15.50, 20.00, 18.75, 22.30 };
            double[] hoursWorked = { 40, 38, 45, 50 };
            #endregion

            #region Salary Calculation

            int employeeIndex = 0;
            do
            {

                double salary = hourlyRates[employeeIndex] * hoursWorked[employeeIndex];


                Console.WriteLine($"Employee: {employeeNames[employeeIndex]}");
                Console.WriteLine($"Hourly Rate: ${hourlyRates[employeeIndex]}");
                Console.WriteLine($"Hours Worked: {hoursWorked[employeeIndex]}");
                Console.WriteLine($"Calculated Salary: ${salary}\n");

                employeeIndex++;

            } while (employeeIndex < employeeNames.Length);
            #endregion
        }

    }
}
