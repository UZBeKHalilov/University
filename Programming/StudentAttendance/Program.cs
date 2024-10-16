//Task 1: Student Attendance Management System

//Description: Create an attendance system that stores student names in an array and marks their
//attendance over several days. Use a do-while loop to collect attendance data for each day and
//an if statement to validate whether a student is present or absent.

//Requirements:

//    Use an array of student names.
//    Track attendance (present/absent) for each student over multiple days.
//    At the end, display the total attendance percentage for each student.

using Microsoft.Data.SqlClient;

namespace StudentAttendance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Student Attendance!");

            StudentDB();
        }

        static void StudentDB()
        {

            string connectionString = $"Server=localhost;Database=StudentDB;User Id=sa;Password={MyKeys.SqlKey};TrustServerCertificate=True;";

            // SQL query to get product list
            string query = "SELECT * FROM Student";

            // Create a connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();
                    Console.WriteLine("Connection to database opened.");

                    // Create a command to execute the query
                    SqlCommand command = new SqlCommand(query, connection);

                    // Execute the query and read the results
                    SqlDataReader reader = command.ExecuteReader();

                    // Check if there are rows
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Students List:");
                        Console.WriteLine("-----------------------------------");

                        // Loop through the data and display the products
                        while (reader.Read())
                        {
                            Console.WriteLine($"Student ID: {reader["StudentID"]}, " +
                                              $"Product Name: {reader["StudentName"]}" );
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Students found.");
                    }

                    // Close the reader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    // Close the connection
                    connection.Close();
                    Console.WriteLine("Connection to database closed.");
                }
            }

        }
    }
}
