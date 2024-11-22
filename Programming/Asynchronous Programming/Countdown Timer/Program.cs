namespace Countdown_Timer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread timerThread = new Thread(Timer);
            timerThread.Start();


            Console.WriteLine("Main thread End!");
        }

        static void Timer()
        {

            for (int i = 15; i >= 0; i--)
            {
                Console.WriteLine($"{i} seconds remain");
                Thread.Sleep( 1000 );
            }
        }
    }
}
