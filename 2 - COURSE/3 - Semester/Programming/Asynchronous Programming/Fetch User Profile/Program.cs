using System;
using System.Threading.Tasks;

namespace Fetch_User_Profile
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Main thread is running...");

            User user = await GetUserProfileAsync();

            user.ShowProfile();

            Console.WriteLine("Main thread is ending...");
        }

        static async Task<User> GetUserProfileAsync()
        {
            Console.WriteLine("Fetching User profile...");
            await Task.Delay(3000);

            User user = new User(1, 18, "UserName");
            Console.WriteLine("User profile fetched.");
            return user;
        }
    }

    class User
    {
        public int ID { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }

        public User(int id, int age, string name)
        {
            ID = id;
            Age = age;
            Name = name;
        }

        public void ShowProfile()
        {
            Console.WriteLine($"User: Name - {Name}, Age - {Age}");
        }
    }
}
