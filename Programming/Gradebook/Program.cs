/*
 Task 4: Gradebook with Array Search

Description: Implement a gradebook that stores students' grades in an array. Allow users to search for a student’s grade and display it. Use a do-while loop for continuous searching and an if statement to validate input.

Requirements:

    Store student names and their corresponding grades in arrays.
    Search for students by name and display their grade.
    Allow multiple searches with an option to exit.
*/

using System;

namespace GradebookSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Arrays to store student names and their corresponding grades
            string[] studentNames = { "Abdulloh", "Miraziz", "Azamat", "Toshmat" };
            int[] studentGrades = { 85, 90, 78, 92 };

            string searchMore = "y";  // To control the do-while loop

            do
            {
                // Ask the user to input a student's name
                Console.WriteLine("Enter the student's name to search for their grade: ");
                string studentName = Console.ReadLine();

                // Flag to check if the student is found
                bool studentFound = false;

                // Search for the student in the array
                for (int i = 0; i < studentNames.Length; i++)
                {
                    if (studentNames[i].Equals(studentName, StringComparison.OrdinalIgnoreCase))
                    {
                        // Student found, display the grade
                        Console.WriteLine($"{studentNames[i]} has a grade of {studentGrades[i]}.");
                        studentFound = true;
                        break;
                    }
                }

                // If the student is not found, display a message
                if (!studentFound)
                {
                    Console.WriteLine("Student not found. Please try again.");
                }

                // Ask if the user wants to search for another student
                Console.WriteLine("Do you want to search for another student? (y/n)");
                searchMore = Console.ReadLine();

            } while (searchMore.ToLower() == "y");

            Console.WriteLine("Exiting the gradebook system. Goodbye!");
        }
    }
}
