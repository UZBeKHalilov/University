
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

namespace VotingCandidatesSystem
{
    internal class Program
    {
        static private readonly string SqlKey = "Server=localhost;Database=StudentDB;User Id=sa;Password=KHalilov#0548;TrustServerCertificate=True;";

        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Voting Candidates System!\n");

            VotingCandidates();
        }

        static void VotingCandidates()
        {

            #region Collecting data from database

            SqlConnection connection = new SqlConnection(SqlKey);

            var candidatesDict = new Dictionary<int, string>();
            var votesDict = new Dictionary<int, int>();

            int votingByConsole;
            int countOfStudents;

            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(StudentID) FROM Students", connection))
                {                    
                    countOfStudents = Convert.ToInt32(command.ExecuteScalar());
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM Candidates", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int candidateId = Convert.ToInt32(reader["CandidateID"]);

                        string candidateName = reader["CandidateName"].ToString() ?? "No Name";


                        candidatesDict.Add(candidateId, candidateName);
                        votesDict.Add(candidateId, 0);
                    }

                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                connection.Close();
            }

            #endregion

            #region Voting candidates

            foreach (var candidate in candidatesDict)
            {
                Console.WriteLine($"ID {candidate.Key}: {candidate.Value}");
            }

            do
            {
                Console.WriteLine("Enter Candidate ID to vote (0 or empty for exit):");

                votingByConsole = int.Parse(Console.ReadLine() ?? "0");

                if (votesDict.ContainsKey(votingByConsole))
                {
                    votesDict[votingByConsole]++;
                    Console.WriteLine($"Voted to {candidatesDict[votingByConsole]}");
                }
                else
                {
                    Console.WriteLine("No such candidate, please re-enter.");
                }

            } while (votingByConsole != 0 && !votesDict.Any(v => v.Value > countOfStudents));
            
            Console.WriteLine("\nVoting is over!\n");

            #endregion

            #region Showing result

            int maxVotes = 0;
            int winner = -1;

            foreach (var entry in votesDict)
            {
                if (entry.Value > maxVotes)
                {
                    maxVotes = entry.Value;
                    winner = entry.Key;
                }
            }

            // Natijani chiqarish
            if (winner != -1)
            {
                Console.WriteLine($"The winner: {candidatesDict[winner]} (Votes: {maxVotes})");
            }
            else
            {
                Console.WriteLine("Winner not found!.");
            }


            Console.WriteLine("\nResults");
            foreach (var candidate in candidatesDict)
            {
                if (votesDict.TryGetValue(candidate.Key, out int voteCount))
                {
                    Console.WriteLine($"Candidate name: {candidate.Value}, Votes: {voteCount}");
                }
            }

            #endregion

            #region Being saved to Database
            try
            {
                connection.Open();
                foreach (var candidate in candidatesDict)
                {
                    if (votesDict.TryGetValue(candidate.Key, out int voteCount))
                    {
                        string query = "UPDATE Candidates SET CandidateName = @CandidateName, VoteCount = @VoteCount, LastVoted = @LastVoted WHERE CandidateID = @CandidateID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@CandidateID", candidate.Key);
                            command.Parameters.AddWithValue("@CandidateName", candidate.Value);
                            command.Parameters.AddWithValue("@VoteCount", voteCount);
                            command.Parameters.AddWithValue("@LastVoted", DateTime.Now);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                Console.WriteLine($"Candidate ID {candidate.Key} yangilandi.");
                            }
                            else
                            {
                                Console.WriteLine($"Candidate ID {candidate.Key} topilmadi.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                connection.Close();
            }
            #endregion
        }
    }
}
