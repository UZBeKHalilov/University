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
            Console.WriteLine("\n\nChoose:\n" +
                "1 - Show all students\n" +
                "2 - Show students attendance\n" +
                "other - EXIT");

            switch (Console.ReadLine())
            {
                case "1":
                       ShowAllStudents();
                    break;

                case "2":
                        ShowStudentsAttendance();
                    break;
                default:
                    Console.WriteLine("\n\n Good bye!!!\n\n");
                    Environment.Exit(0);
                    break;
            }

            StudentDB();
        }

        static void ShowAllStudents()
        {
            string connectionString = MyKeys.GetSqlConnectionString();

            // SQL query to get product list
            string query = "SELECT * FROM Students";

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
                                              $"Student Name: {reader["StudentName"]}" );
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
    
        static void ShowStudentsAttendance() 
        {
            string connectionString = MyKeys.GetSqlConnectionString();

            // SQL query to get product list
            string studentsQuery = "SELECT StudentID, StudentName FROM Students";
            string attendanceQuery = "SELECT * FROM Attendance";

            // Create a connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();
                    Console.WriteLine("Connection to database opened.");

                    // Create a command to execute the query
                    SqlCommand studentsCommand = new SqlCommand(studentsQuery, connection);
                    SqlCommand attendanceCommand = new SqlCommand(attendanceQuery, connection);

                    // Execute the query and read the results
                    SqlDataReader studentsTableReader = studentsCommand.ExecuteReader();
                    SqlDataReader attendanceTableReader = attendanceCommand.ExecuteReader();

                    // Check if there are rows
                    
                    if (!studentsTableReader.HasRows)
                    {
                        Console.WriteLine("No students found!");
                        return;
                    }



                    // Close the reader
                    studentsTableReader.Close();
                    attendanceTableReader.Close();
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
