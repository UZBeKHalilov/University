using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendance
{
    static public class MyKeys
    {
        private static string SqlPassword = "KHalilov#0548";

        public static string GetSqlConnectionString()
        {
            return $"Server=localhost;Database=StudentDB;User Id=sa;Password={SqlPassword};TrustServerCertificate=True;" ;
        }
    }
}
