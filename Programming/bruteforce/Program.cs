using MySql.Data.MySqlClient;

class Program
{
    private static string connectionString = "Server=localhost;Port=3307;Database=test_db;Uid=root;Pwd=root;";
    
    static void Main(string[] args)
    {
        string targetUsername = "admin";
        string[] possiblePasswords = { "admin", "password", "admin13", "letmein", "root" };

        foreach (string password in possiblePasswords)
        {
            if (TryLogin(targetUsername, password))
            {
                Console.WriteLine($"[+] Password found: {password}");
                break;
            }
            else
            {
                Console.WriteLine($"[-] Incorrect password: {password}");
            }
        }

        // Common patterns and strategies
        var commonWords = new[] { "password", "admin", "user", "letmein", "welcome" };
        var leetMap = new Dictionary<char, string>
        {
            { 'a', "@" },
            { 'e', "3" },
            { 'i', "1" },
            { 'o', "0" },
            { 's', "$" },
            { 't', "7" }
        };
        var commonSuffixes = new[] { "123", "2024", "!", "@" };

        // Generate passwords
        foreach (var password in GenerateSmartPasswords(commonWords, leetMap, commonSuffixes))
        {
            Console.WriteLine(password);
        }
    }
      static IEnumerable<string> GenerateSmartPasswords(string[] commonWords, Dictionary<char, string> leetMap, string[] commonSuffixes)
    {
        var generatedPasswords = new HashSet<string>(); // Avoid duplicates

        // 1. Plain common words
        foreach (var word in commonWords)
        {
            if (generatedPasswords.Add(word))
                yield return word;
        }

        // 2. Apply leetspeak substitutions
        foreach (var word in commonWords)
        {
            var leetWord = ApplyLeetSpeak(word, leetMap);
            if (generatedPasswords.Add(leetWord))
                yield return leetWord;
        }

        // 3. Combine common words with suffixes
        foreach (var word in commonWords)
        {
            foreach (var suffix in commonSuffixes)
            {
                var combined = word + suffix;
                if (generatedPasswords.Add(combined))
                    yield return combined;
            }
        }

        // 4. Mix leetspeak words with suffixes
        foreach (var word in commonWords)
        {
            var leetWord = ApplyLeetSpeak(word, leetMap);
            foreach (var suffix in commonSuffixes)
            {
                var combined = leetWord + suffix;
                if (generatedPasswords.Add(combined))
                    yield return combined;
            }
        }
    }

    static string ApplyLeetSpeak(string word, Dictionary<char, string> leetMap)
    {
        var leetWord = string.Empty;
        foreach (var c in word)
        {
            leetWord += leetMap.ContainsKey(c) ? leetMap[c] : c.ToString();
        }
        return leetWord;
    }

    static bool TryLogin(string username, string password)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string selectQuery = "SELECT password, failed_attempts FROM users WHERE username = @username";
                using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@username", username);

                    using (var reader = selectCommand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("User not found.");
                            return false;
                        }

                        reader.Read();
                        int failedAttempts = reader.GetInt32(reader.GetOrdinal("failed_attempts"));
                        string storedPassword = reader.GetString(reader.GetOrdinal("password"));

                        if (failedAttempts >= 5)
                        {
                            Console.WriteLine("Account locked. Too many failed attempts.");
                            return false;
                        }

                        if (storedPassword == password)
                        {
                            reader.Close(); // Ensure the reader is closed before further operations
                            ResetFailedAttempts(username, connection);
                            return true;
                        }
                        else
                        {
                            reader.Close(); // Close the reader before updating
                            IncrementFailedAttempts(username, connection);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }

    static void ResetFailedAttempts(string username, MySqlConnection connection)
    {
        string updateQuery = "UPDATE users SET failed_attempts = 0 WHERE username = @username";
        using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
        {
            updateCommand.Parameters.AddWithValue("@username", username);
            updateCommand.ExecuteNonQuery();
        }
    }

    static void IncrementFailedAttempts(string username, MySqlConnection connection)
    {
        string updateQuery = "UPDATE users SET failed_attempts = failed_attempts + 1 WHERE username = @username";
        using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
        {
            updateCommand.Parameters.AddWithValue("@username", username);
            updateCommand.ExecuteNonQuery();
        }
    }
}
