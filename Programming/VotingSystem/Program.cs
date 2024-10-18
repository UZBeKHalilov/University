
//Task 2: Voting System for Class President

//Description: Create a voting system where students vote for a class president. The system should accept a list of candidates and then
//allow students to vote multiple times using an array and do-while loop. Count and display the votes after the election.

//Requirements:

//    Store candidate names in an array.
//    Use a do-while loop to take votes until all students have voted.
//    Display the winner based on the highest vote count using an if statement.

// --------------------------------------------------------------------------------------------

//2 - topshiriq: Sinf rahbari uchun ovoz berish tizimi

//Tavsif: O'quvchilar sinf rahbariga ovoz beradigan ovoz berish tizimini yarating. Tizim nomzodlar roʻyxatini qabul qilishi,
//soʻngra talabalarga massiv va do-while siklidan foydalanib bir necha marta ovoz berishga ruxsat berishi kerak.
//Saylovdan keyin ovozlarni sanash va ko'rsatish.

//Talablar:

// Nomzod nomlarini massivda saqlang.
// Barcha talabalar ovoz bermaguncha ovoz berish uchun do-while siklidan foydalaning.
// If iborasidan foydalangan holda eng yuqori ovozlar soni asosida g'olibni ko'rsating.

using Microsoft.Data.SqlClient;

namespace VotingSystem
{
    internal class Program
    {
        static private readonly string SqlKey = "Server=localhost;Database=StudentDB;User Id=sa;Password=KHalilov#0548;TrustServerCertificate=True;";
      
        static void Main(string[] args)
        {
            string connectionString = SqlKey;
            SqlConnection connection = new SqlConnection(connectionString);

            int totalStudents = 5; // Talabalar soni
            int[] votes = new int[totalStudents]; // Ovozlar uchun massiv
            int vote;
            int numberOfVotes = 0;

            // Ovoz berish jarayoni
            do
            {
                Console.WriteLine("Ovoz berish uchun nomzodlar:");
                using (SqlCommand command = new SqlCommand("SELECT * FROM Candidates", connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["CandidateID"]}: {reader["CandidateName"]}");
                    }
                    connection.Close();
                }

                Console.Write("Ovoz berish uchun nomzod ID raqamini kiriting (0 orqali tugatish): ");
                vote = Convert.ToInt32(Console.ReadLine());

                if (vote > 0)
                {
                    votes[numberOfVotes] = vote;
                    numberOfVotes++;
                }

            } while (vote != 0 && numberOfVotes < totalStudents);

            // Ovozlarni sanash
            int[] voteCounts = new int[3]; // Nomzodlar soni bo'yicha

            foreach (var v in votes)
            {
                if (v > 0)
                {
                    voteCounts[v - 1]++;
                }
            }

            // Natijalarni ko'rsatish
            int maxVotes = 0;
            int winnerId = -1;

            for (int i = 0; i < voteCounts.Length; i++)
            {
                if (voteCounts[i] > maxVotes)
                {
                    maxVotes = voteCounts[i];
                    winnerId = i + 1;
                }
            }

            if (winnerId != -1)
            {
                Console.WriteLine($"G'olib: {winnerId} raqamli nomzod");
            }
        }
    }
}
