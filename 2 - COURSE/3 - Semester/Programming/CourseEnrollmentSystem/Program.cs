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
        struct Course
        {
            public string CourseName;
            public int MaxCapacity;
            public int CurrentEnrollment;
        }

        static void Main()
        {

            CourseEntrollment();            
        }
    
        static void CourseEntrollment()
        {
            Course[] courses = new Course[]
              {
                new Course { CourseName = "Software Engineering", MaxCapacity = 5, CurrentEnrollment = 0 },
                new Course { CourseName = "Artificial Intelligence", MaxCapacity = 5, CurrentEnrollment = 0 },
                new Course { CourseName = "Big Data", MaxCapacity = 5, CurrentEnrollment = 0 },
                new Course { CourseName = "Business Process Management", MaxCapacity = 5, CurrentEnrollment = 0 }
              };

            // Array to store student names
            string[] students = new string[100];
            int studentCount = 0;

            // Enrollment loop
            string enrollMore = "y";
            do
            {
                Console.WriteLine("Enter student name: ");
                string studentName = Console.ReadLine();

                // Display available courses
                Console.WriteLine("Available Courses:");
                for (int i = 0; i < courses.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {courses[i].CourseName} - Capacity: {courses[i].CurrentEnrollment}/{courses[i].MaxCapacity}");
                }

                // Select course
                Console.WriteLine("Select a course number to enroll in:");
                int courseNumber = Convert.ToInt32(Console.ReadLine()) - 1;

                // Check if the course is full
                if (courses[courseNumber].CurrentEnrollment < courses[courseNumber].MaxCapacity)
                {
                    // Enroll the student
                    courses[courseNumber].CurrentEnrollment++;
                    students[studentCount++] = studentName;
                    Console.WriteLine($"{studentName} has been enrolled in {courses[courseNumber].CourseName}.");
                }
                else
                {
                    Console.WriteLine("Sorry, the course is full. Please choose a different course.");
                }

                // Ask if the user wants to enroll more students
                Console.WriteLine("Do you want to enroll another student? (y/n)");
                enrollMore = Console.ReadLine();

            } while (enrollMore.ToLower() == "y");

            // Print final enrollment list
            Console.WriteLine("\nFinal Enrollment List:");
            for (int i = 0; i < courses.Length; i++)
            {
                Console.WriteLine($"{courses[i].CourseName}: {courses[i].CurrentEnrollment}/{courses[i].MaxCapacity}");
            }
        }
    }
    }



}
