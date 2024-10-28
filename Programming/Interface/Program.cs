namespace Interface
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n\nTask - 1\n\n");
            
            IVehicle Car = new Car();
            
            Car.StopEngine();
            Car.StartEngine();

            Motorsicle Motor = new Motorsicle();
            
            Motor.StartEngine();
            Motor.StopEngine();
            
            // ----- Task 2 ----- \\
            Console.WriteLine("\n\nTask - 2\n\n");

            var Bird = new Bird();
            Bird.Fly();
            Bird.MakeSound(); 

            // ----- Task 3 ----- \\
            Console.WriteLine("\n\nTask - 3\n\n");

            var Ractangle = new Ractangle(10, 20);

            Console.WriteLine(Ractangle.GetArea());

            var Circle = new Circle(10);
            Console.WriteLine(Circle.GetArea());


            // ----- Task 4 ----- \\
            Console.WriteLine("\n\nTask - 4\n\n");
            
            Document File = new Document();
            
            File.Read();
            File.Write("Something");
            File.Read();


            // ----- Task 5 ----- \\
            Console.WriteLine("\n\nTask - 5\n\n");
            
            var NewStudent = new Student("Abdulloh", 18);
            NewStudent.Print();

            // ----- Task 6 ----- \\
            Console.WriteLine("\n\nTask - 6\n\n");
            

            ICalculator calc = new Addition();
            Console.WriteLine(calc.Calculate(56,4));

            calc = new Subtraction();
            Console.WriteLine(calc.Calculate(56,6));

            // ----- Task 7 ----- \\
            Console.WriteLine("\n\nTask - 7\n\n");
            
            FlyingCar FlyCar = new FlyingCar();
            FlyCar.Drive();
            FlyCar.Fly();


            // ----- Task 8 ----- \\
            Console.WriteLine("\n\nTask - 8\n\n");

            Console.WriteLine("Need help from Teacher. ChatGPT can't explain it to me xD");
            

            // ----- Task 9 ----- \\
            Console.WriteLine("\n\nTask - 9\n\n");

            IClickable button = new Button();
            IClickable hyperlink = new Hyperlink();

            // Simulate clicking the button
            Console.WriteLine("Simulating Button Click:");
            SimulateClick(button);

            // Simulate clicking the hyperlink
            Console.WriteLine("\nSimulating Hyperlink Click:");
            SimulateClick(hyperlink);


            // ----- Task 10 ----- \\
            Console.WriteLine("\n\nTask - 10\n\n");

            string filePath = "C:\\Users\\Abdulloh\\source\\repos\\Interface\\File.txt";
            IFileOperation fileOperation = new TextFileOperation();

            Console.WriteLine("Writing to the file...");
            fileOperation.WriteFile(filePath, "This is a sample text content.");

            // Read from the file
            Console.WriteLine("\nReading from the file...");
            fileOperation.ReadFile(filePath);

        }

        static void SimulateClick(IClickable clickable)
        {
            clickable.Click();
        }
    }
}
