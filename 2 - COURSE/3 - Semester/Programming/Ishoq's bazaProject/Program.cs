using System;
using Microsoft.Data.SqlClient;

namespace YourProjectName
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=master;User Id=SA;Password=IsoqUsmonov@1989yil;TrustServerCertificate=True;";
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("SQL Serverga ulanish muvaffaqiyatli!");

                    // Jadval yaratish
                    string createTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
                    BEGIN
                        CREATE TABLE Products (
                            Id INT PRIMARY KEY IDENTITY(1,1),
                            Name NVARCHAR(100),
                            PriceInUZS DECIMAL(10, 2),
                            PriceInUSD DECIMAL(10, 2)
                        );
                    END";
                    
                    using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Products jadvali yaratildi yoki allaqachon mavjud.");
                    }

                    // Ma'lumot qo'shish
                    string insertDataQuery = "INSERT INTO Products (Name, PriceInUZS, PriceInUSD) VALUES ('Savat', 1000.00, 100.00);";
                    using (SqlCommand command = new SqlCommand(insertDataQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Ma'lumot Products jadvaliga qo'shildi.");
                    }

                    // Ma'lumotlarni o'qish
                    string selectDataQuery = "SELECT * FROM Products;";
                    using (SqlCommand command = new SqlCommand(selectDataQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Jadval ma'lumotlari:");
                            while (reader.Read())
                            {
                                Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}, PriceInUZS: {reader["PriceInUZS"]}, PriceInUSD: {reader["PriceInUSD"]}");
                            }
                        }
                    }

                    // Ma'lumot yangilash
                    string updateDataQuery = "UPDATE Products SET PriceInUZS = 1200.00, PriceInUSD = 120.00 WHERE Name = 'Savat';";
                    using (SqlCommand command = new SqlCommand(updateDataQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Ma'lumot yangilandi.");
                    }

                    // Yangilangan ma'lumotlarni o'qish
                    using (SqlCommand command = new SqlCommand(selectDataQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Yangilangan jadval ma'lumotlari:");
                            while (reader.Read())
                            {
                                Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}, PriceInUZS: {reader["PriceInUZS"]}, PriceInUSD: {reader["PriceInUSD"]}");
                            }
                        }
                    }

                    // Ma'lumot o'chirish
                    string deleteDataQuery = "DELETE FROM Products WHERE Name = 'Savat';";
                    using (SqlCommand command = new SqlCommand(deleteDataQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Ma'lumot o'chirildi.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Xato: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
