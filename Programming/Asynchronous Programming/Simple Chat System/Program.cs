namespace Simple_Chat_System
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    class Program
    {
        private static ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
        private static bool isRunning = true;

        static void Main(string[] args)
        {
            Thread listenerThread = new Thread(ListenMessages);
            Thread senderThread = new Thread(SendMessages);

            listenerThread.Start();
            senderThread.Start();

            senderThread.Join();

            isRunning = false;
            listenerThread.Join();

            Console.WriteLine("Chat system terminated.");
        }

        private static void ListenMessages()
        {
            while (isRunning)
            {
                if (messageQueue.TryDequeue(out string message))
                {
                    Console.WriteLine($"\nReceived: {message}");
                }

                Thread.Sleep(1000);
            }
        }

        private static void SendMessages()
        {
            while (isRunning)
            {
                Console.Write("Enter a message to send (type 'exit' to quit): ");
                string message = Console.ReadLine();

                if (message?.ToLower() == "exit")
                {
                    isRunning = false;
                    break;
                }

                messageQueue.Enqueue(message);
            }
        }
    }

}
