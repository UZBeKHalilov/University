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

            // SQL query to get the list of students and their attendance
            string studentsQuery = "SELECT StudentID, StudentName FROM Students";
            string attendanceQuery = "SELECT StudentID, COUNT(*) AS TotalDays, SUM(CASE WHEN IsPresent = 1 THEN 1 ELSE 0 END) AS PresentDays FROM Attendance GROUP BY StudentID";

            // Create a connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();
                    Console.WriteLine("\nConnection to database opened.\n");

                    // Create commands to execute the queries
                    SqlCommand studentsCommand = new SqlCommand(studentsQuery, connection);
                    SqlCommand attendanceCommand = new SqlCommand(attendanceQuery, connection);

                    // Execute the queries and get the results
                    SqlDataReader studentsTableReader = studentsCommand.ExecuteReader();

                    // Temporary storage for student data (using Dictionary now)
                    var studentsDict = new Dictionary<int, string>();

                    // Read student names and IDs
                    while (studentsTableReader.Read())
                    {
                        int studentId = (int)studentsTableReader["StudentID"];
                        string studentName = studentsTableReader["StudentName"].ToString();
                        studentsDict[studentId] = studentName;  // Storing in Dictionary
                    }

                    // Close the reader
                    studentsTableReader.Close();

                    // Now read attendance data
                    SqlDataReader attendanceTableReader = attendanceCommand.ExecuteReader();

                    // Temporary storage for attendance data
                    var attendanceDataDict = new Dictionary<int, (int TotalDays, int PresentDays)>();

                    while (attendanceTableReader.Read())
                    {
                        int studentId = (int)attendanceTableReader["StudentID"];
                        int totalDays = (int)attendanceTableReader["TotalDays"];
                        int presentDays = (int)attendanceTableReader["PresentDays"];
                        attendanceDataDict[studentId] = (totalDays, presentDays);
                    }

                    // Close the reader
                    attendanceTableReader.Close();

                    // Display results
                    Console.WriteLine("Student Attendance Report:");
                    foreach (var studentId in studentsDict.Keys)
                    {
                        string studentName = studentsDict[studentId];

                        if (attendanceDataDict.ContainsKey(studentId))
                        {
                            var (totalDays, presentDays) = attendanceDataDict[studentId];
                            double attendancePercentage = (double)presentDays / totalDays * 100;
                            Console.WriteLine($"{studentName}: {attendancePercentage:F2}% attendance ({presentDays}/{totalDays} days present)");
                        }
                        else
                        {
                            Console.WriteLine($"{studentName}: No attendance data found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    // Close the connection
                    connection.Close();
                    Console.WriteLine("\nConnection to database closed.");
                }
            }
        }
    }
}
