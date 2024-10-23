using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Account_System
{
    internal class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public User(string userName, string password) 
        {
            UserName = userName;
            Password = password;
        }


    }
}
