using System;
using System.Collections.Generic; // For List<T>
using Microsoft.Data.SqlClient;

namespace GradebookSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Gradebook();
        }

        static void Gradebook()
        {
            #region Database connection
            // SQL connection string
            SqlConnection connection = new SqlConnection("Server=localhost;Database=StudentDB;User Id=sa;Password=KHalilov#0548;TrustServerCertificate=True;");
            #endregion

            #region Initialize lists to store student names and grades
            // Lists to store student names and their corresponding grades
            List<string> studentNames = new List<string>();
            List<int> studentGrades = new List<int>();
            #endregion

            #region Collect data from the database
            try
            {
                // Open connection to the database
                connection.Open();

                // Write the SQL query to join Students and Grades
                using (var command = new SqlCommand("SELECT s.StudentName, g.Grade FROM Students s JOIN Grades g ON s.StudentID = g.StudentID", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    // Read data from the query result
                    while (reader.Read())
                    {
                        // Fetch student name and grade
                        string studentName = reader["StudentName"].ToString();
                        int grade = Convert.ToInt32(reader["Grade"]);

                        // Add the fetched data to the respective lists
                        studentNames.Add(studentName);
                        studentGrades.Add(grade);
                    }
                }

                // Close the database connection after fetching the data
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return; // Stop the program in case of an error
            }
            #endregion

            #region Search functionality
            // Do-while loop to allow continuous searching
            string searchMore = "y";  // To control the do-while loop

            do
            {
                // Ask the user to input a student's name
                Console.WriteLine("Enter the student's name to search for their grade: ");
                string studentName = Console.ReadLine();

                // Flag to check if the student is found
                bool studentFound = false;

                // Search for the student in the list
                for (int i = 0; i < studentNames.Count; i++)
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
            #endregion
        }
    }
}
