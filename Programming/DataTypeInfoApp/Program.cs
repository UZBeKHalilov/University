using Microsoft.Data.SqlClient;

namespace DataTypeInfoApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Integer types
            Console.WriteLine($"byte MIN = {byte.MinValue} | MAX = {byte.MaxValue}");
            Console.WriteLine($"sbyte MIN = {sbyte.MinValue} | MAX = {sbyte.MaxValue}");
            Console.WriteLine($"short MIN = {short.MinValue} | MAX = {short.MaxValue}");
            Console.WriteLine($"ushort MIN = {ushort.MinValue} | MAX = {ushort.MaxValue}");
            Console.WriteLine($"int MIN = {int.MinValue} | MAX = {int.MaxValue}");
            Console.WriteLine($"uint MIN = {uint.MinValue} | MAX = {uint.MaxValue}");
            Console.WriteLine($"long MIN = {long.MinValue} | MAX = {long.MaxValue}");
            Console.WriteLine($"ulong MIN = {ulong.MinValue} | MAX = {ulong.MaxValue}");

            // Floating-point types
            Console.WriteLine($"float MIN = {float.MinValue} | MAX = {float.MaxValue}");
            Console.WriteLine($"double MIN = {double.MinValue} | MAX = {double.MaxValue}");
            Console.WriteLine($"decimal MIN = {decimal.MinValue} | MAX = {decimal.MaxValue}");

            // Character type
            Console.WriteLine($"char MIN = {(int)char.MinValue} | MAX = {(int)char.MaxValue}");

            // Boolean type
            Console.WriteLine($"bool MIN = {false} | MAX = {true}");

            // String type (no min/max)
            Console.WriteLine("string MIN = N/A | MAX = N/A");

            // Object type (no min/max)
            Console.WriteLine("object MIN = N/A | MAX = N/A");

            // Dynamic type (no min/max)
            Console.WriteLine("dynamic MIN = N/A | MAX = N/A");
        }
    }
}
