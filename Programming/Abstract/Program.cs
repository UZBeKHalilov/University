//Task 1: Abstract Class for Animals

//Create an abstract class Animal with an abstract method MakeSound(). Then, create two derived classes Dog and Cat that
//implement the MakeSound() method. Instantiate both classes and call the MakeSound() method.
//Task 2: Banking System

//Create an abstract class Account with an abstract method Withdraw().
//Define a derived class SavingsAccount that implements the Withdraw()
//method with specific rules (e.g., balance should not go below $100).
//Test your implementation by creating an object of SavingsAccount.

//Task 3: Vehicle System

//Design an abstract class Vehicle with an abstract method StartEngine().
//Create two derived classes Car and Motorbike that provide their own implementations for StartEngine().
//Ensure that the base class also has a concrete method DisplayInfo() that prints information about the vehicle.

//Task 4: Shape Area Calculation

//Create an abstract class Shape with an abstract method CalculateArea().
//Then, implement two derived classes Circle and Rectangle that override CalculateArea().
//Calculate the area for different shapes using a base class reference.

//Task 5: E - commerce Orders

//Create an abstract class Order with an abstract method ProcessOrder().
//Implement two derived classes OnlineOrder and InStoreOrder where each implements the ProcessOrder()
//method in a different way (e.g., online payment for OnlineOrder, and cash for InStoreOrder).

//Task 6: Employee Management System

//Design an abstract class Employee with an abstract method CalculateSalary().
//Then, create derived classes FullTimeEmployee and PartTimeEmployee, each implementing the CalculateSalary()
//method based on different rules (e.g., monthly salary vs. hourly wage).

//Task 7: Appliance Control System

//Create an abstract class Appliance with an abstract method TurnOn() and a concrete method TurnOff().
//Create derived classes WashingMachine and AirConditioner, each implementing TurnOn(). Test turning the appliances on and off using these classes.

//Task 8: Ticket Booking System

//Define an abstract class Ticket with an abstract method GenerateTicket(). Implement two derived classes BusTicket and FlightTicket, each with its own logic for generating the ticket. Test the ticket generation process using these derived classes.
//Task 9: Food Ordering System

//Create an abstract class Food with an abstract method Prepare(). Implement the derived classes Pizza and Salad, where each provides its own recipe in the Prepare() method. Demonstrate the preparation process using base class references.
//Task 10: School System

//Create an abstract class Person with an abstract method GetDetails(). Implement two derived classes Student and Teacher where each overrides GetDetails() to return the respective details. Instantiate these classes and print the details of students and teachers.

namespace Abstract
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Animals Dog = new Dog();
            Dog.MakeSound();

            Animals Cat = new Cat();
            Cat.MakeSound();
        }
    }
}
