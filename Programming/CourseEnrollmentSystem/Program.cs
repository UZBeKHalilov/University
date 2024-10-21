//Task 3: Course Enrollment System

//Description: Develop a system to allow students to enroll in courses. Each course can have a maximum capacity.
//The system should allow students to enroll in available courses using an if statement to check capacity and a do-while loop to take multiple enrollments.

//Requirements:

//    Use an array to store courses and their available seats.
//    Check course availability before enrolling a student.
//    Print the final list of students enrolled in each course.

/*
3-Topshiriq: Kursga qabul qilish tizimi

Tavsif: Talabalarga kurslarga yozilish imkonini beruvchi tizimni ishlab chiqish. Har bir kurs maksimal quvvatga ega bo'lishi mumkin. 
Tizim talabalarga imkoniyatlarni tekshirish uchun if ko'rsatmasi va bir nechta ro'yxatga olish uchun do-while siklidan 
foydalangan holda mavjud kurslarga yozilish imkonini berishi kerak.

Talablar:

 Kurslar va ularning mavjud o'rindiqlarini saqlash uchun massivdan foydalaning.
 Talabani ro'yxatga olishdan oldin kurs mavjudligini tekshiring.
 Har bir kursga ro'yxatdan o'tgan talabalarning yakuniy ro'yxatini chop eting.
*/

using Microsoft.Data.SqlClient;

namespace CourseEnrollmentSystem
{
    internal class CourseEnrollmentSystem
    {
        static private readonly string SqlKey = "Server=localhost;Database=StudentDB;User Id=sa;Password=KHalilov#0548;TrustServerCertificate=True;";
        static void Main()
        {

            CourseEntrollment();            
        }
    
        static void CourseEntrollment()
        {
          
            #region Collecting data from database
            SqlConnection connection = new SqlConnection(SqlKey);

            int[] maxCapacity = { 2, 3, 1 }; 
            string[][] enrolledStudents = new string[maxCapacity.Length][];
            int[] currentEnrollment = new int[maxCapacity.Length]; 

            int courseCount = 0;

            using (connection)
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Courses", connection))
                    {
                        courseCount = (int)command.ExecuteScalar(); // Get the number of courses
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return;
                }
                finally
                {
                    connection.Close();
                }
            }

            string[] courses = new string[courseCount];

            using (connection)
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT CourseName FROM Courses", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        int index = 0;

                        while (reader.Read())
                        {
                            courses[index] = reader["CourseName"].ToString(); // Populate the array
                            index++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            // Example output to verify courses are loaded
            Console.WriteLine("Courses available:");
            foreach (var course in courses)
            {
                Console.WriteLine(course);
            }

            #endregion

            // Define courses and their maximum capacity

            // Initialize enrolled students arrays
            for (int i = 0; i < courses.Length; i++)
            {
                enrolledStudents[i] = new string[maxCapacity[i]];
            }

            // Initialize the continueEnrollment variable
            string continueEnrollment = "yes"; // Default value to enter the loop
            do
            {
                Console.WriteLine("Available Courses:");
                for (int i = 0; i < courses.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {courses[i]} (Available Seats: {maxCapacity[i] - currentEnrollment[i]})");
                }

                Console.Write("Select a course by number (1-3): ");
                int courseIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                if (courseIndex < 0 || courseIndex >= courses.Length)
                {
                    Console.WriteLine("Invalid course selection. Please try again.");
                    continue;
                }

                // Check if there are available seats in the selected course
                if (currentEnrollment[courseIndex] < maxCapacity[courseIndex])
                {
                    Console.Write("Enter student name: ");
                    string studentName = Console.ReadLine();

                    // Enroll the student
                    enrolledStudents[courseIndex][currentEnrollment[courseIndex]] = studentName;
                    currentEnrollment[courseIndex]++;
                    Console.WriteLine($"{studentName} has been enrolled in {courses[courseIndex]}.");
                }
                else
                {
                    Console.WriteLine($"Sorry, {courses[courseIndex]} is full. No available seats.");
                }

                Console.Write("Do you want to enroll another student? (yes/no): ");
                continueEnrollment = Console.ReadLine().ToLower();

            } while (continueEnrollment == "yes");

            // Print final list of enrolled students in each course
            Console.WriteLine("\nFinal Enrollment List:");
            for (int i = 0; i < courses.Length; i++)
            {
                Console.WriteLine($"\n{courses[i]}: ");
                for (int j = 0; j < currentEnrollment[i]; j++)
                {
                    Console.WriteLine($"- {enrolledStudents[i][j]}");
                }

                if (currentEnrollment[i] == 0)
                {
                    Console.WriteLine("No students enrolled.");
                }
            }
        }
    }



}
