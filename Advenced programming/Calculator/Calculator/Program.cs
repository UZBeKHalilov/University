namespace Calculator
{
    internal class Program
    {
        /// <summary>
        /// Calculator
        /// 1) Procedure
        /// 2) Imperative
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose calculator style:");
                Console.WriteLine("1) Procedural");
                Console.WriteLine("2) Imperative (stateful)");
                Console.WriteLine("0) Exit");
                Console.Write("Select: ");

                var choice = Console.ReadLine();
                if (choice == "0") return;

                if (choice == "1") RunProcedural();
                else if (choice == "2") RunImperative();
                else
                {
                    Console.WriteLine("Invalid choice. Press Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        // Procedural-style calculator: pure functions that take inputs and return a result
        private static void RunProcedural()
        {
            Console.Clear();
            Console.WriteLine("Procedural calculator (two-operand)");

            double a = ReadDouble("Enter first number: ");
            double b = ReadDouble("Enter second number: ");

            Console.WriteLine("Choose operation: + - * /");
            Console.Write("Operation: ");
            var op = Console.ReadLine();

            try
            {
                double res = op switch
                {
                    "+" => Procedural.Add(a, b),
                    "-" => Procedural.Subtract(a, b),
                    "*" => Procedural.Multiply(a, b),
                    "/" => Procedural.Divide(a, b),
                    _ => throw new InvalidOperationException("Unknown operator")
                };

                Console.WriteLine($"Result: {res}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.WriteLine("Press Enter to return to menu...");
            Console.ReadLine();
        }

        // Imperative-style calculator: an object that keeps state and operations mutate that state
        private static void RunImperative()
        {
            Console.Clear();
            Console.WriteLine("Imperative calculator (stateful). You can chain operations on a stored value.");

            double initial = ReadDouble("Enter initial value: ");
            var calc = new ImperativeCalculator(initial);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"Current value: {calc.Value}");
                Console.WriteLine("Operations: + - * / | c = clear | q = quit to menu");
                Console.Write("Enter operation followed by a number (e.g. + 5): ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var op = parts[0];

                if (op.Equals("q", StringComparison.OrdinalIgnoreCase)) break;
                if (op.Equals("c", StringComparison.OrdinalIgnoreCase))
                {
                    calc.Clear();
                    Console.WriteLine("Value cleared to 0.");
                    continue;
                }

                if (parts.Length < 2)
                {
                    Console.WriteLine("Please provide an operation and a number, e.g. + 2");
                    continue;
                }

                if (!double.TryParse(parts[1], out var operand))
                {
                    Console.WriteLine("Invalid number.");
                    continue;
                }

                try
                {
                    switch (op)
                    {
                        case "+": calc.Add(operand); break;
                        case "-": calc.Subtract(operand); break;
                        case "*": calc.Multiply(operand); break;
                        case "/": calc.Divide(operand); break;
                        default: Console.WriteLine("Unknown operation"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        private static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (double.TryParse(s, out var d)) return d;
                Console.WriteLine("Invalid number, try again.");
            }
        }
    }

    static class Procedural
    {
        public static double Add(double a, double b) => a + b;
        public static double Subtract(double a, double b) => a - b;
        public static double Multiply(double a, double b) => a * b;
        public static double Divide(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Cannot divide by zero.");
            return a / b;
        }
    }

    class ImperativeCalculator
    {
        public double Value { get; private set; }

        public ImperativeCalculator(double initial = 0) => Value = initial;

        public void Add(double x) => Value += x;
        public void Subtract(double x) => Value -= x;
        public void Multiply(double x) => Value *= x;
        public void Divide(double x)
        {
            if (x == 0) throw new DivideByZeroException("Cannot divide by zero.");
            Value /= x;
        }

        public void Clear() => Value = 0;
    }
    }
}
