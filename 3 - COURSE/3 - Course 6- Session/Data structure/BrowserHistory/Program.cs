using System;

namespace BrowserHistory
{
    internal class Program
    {
        private static BrowserHistory BrowserHistory = new BrowserHistory();
        private static BrowserHistory FavouriteHistory = new BrowserHistory();
        static void Main(string[] args)
        {

            BrowserHistory.Visit("google.com");
            BrowserHistory.Visit("github.com");
            BrowserHistory.Visit("stackoverflow.com");
            BrowserHistory.Visit("dotnet.microsoft.com");

            FavouriteHistory.Visit("Telegram.com");
            FavouriteHistory.Visit("Youtube.com");
            FavouriteHistory.Visit("hHrome.com");



            Console.WriteLine("Current Page: " + BrowserHistory.GetCurrentPage());

            BrowserHistory.GoBack();
            Console.WriteLine("After Going Back, Current Page: " + BrowserHistory.GetCurrentPage());

            BrowserHistory.GoForward();
            Console.WriteLine("After Going Forward, Current Page: " + BrowserHistory.GetCurrentPage());

            ChooseOptions(BrowserHistory);
        }

        static void ChooseOptions(BrowserHistory history)
        {
            Console.WriteLine("\n\nChoose an option:");
            Console.WriteLine("1. Visit a new page");
            Console.WriteLine("2. Go back");
            Console.WriteLine("3. Go forward");
            Console.WriteLine("4. Get Current Page");
            Console.WriteLine("5. Add to Favourites");
            Console.WriteLine("6. Switch to Favourites");
            Console.WriteLine("7. Switch to Browser tab");
            Console.WriteLine("Any. Exit");

            switch (Console.ReadLine())
            {

                case "1":
                    Console.WriteLine("Enter the URL to visit:");
                    string url = Console.ReadLine() ?? string.Empty;
                    history.Visit(url);

                    break;

                case "2":
                    Console.WriteLine("Going back...");
                    history.GoBack();
                    DisplayCurrentPage(history);
                    break;

                case "3":
                    Console.WriteLine("Going forward...");
                    history.GoForward();
                    DisplayCurrentPage(history);
                    break;

                case "4":
                    Console.WriteLine("Getting current page...");
                    DisplayCurrentPage(history);
                    break;

                case "5":
                    Console.WriteLine("Adding current page to favourites...");
                    string? currentPage = BrowserHistory.GetCurrentPage();
                    if (currentPage != null)
                    {
                        FavouriteHistory.Visit(currentPage);
                        Console.WriteLine("Added to favourites: " + currentPage);
                    }
                    else
                    {
                        Console.WriteLine("No current page to add to favourites.");
                    }
                    break;

                case "6":
                    Console.WriteLine("Switching to Favourites...");
                    ChooseOptions(FavouriteHistory);
                    return;

                case "7":
                    Console.WriteLine("Switching to Browser tab...");
                    ChooseOptions(BrowserHistory);
                    return;

                default:
                    Console.WriteLine("Exiting...");
                    return;
            }
            Console.WriteLine("\n");
            ChooseOptions(history);
        }

        static void DisplayCurrentPage(BrowserHistory history)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nCurrent Page: " + history.GetCurrentPage());
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public class BrowserHistory
    {
        private class Node
        {
            public string Url { get; set; }
            public Node? Previous { get; set; }
            public Node? Next { get; set; }

            public Node(string url)
            {
                Url = url;
            }
        }

        private Node? current;

        public void Visit(string url)
        {
            Node newNode = new Node(url);

            if (current != null)
            {
                current.Next = newNode;
                newNode.Previous = current;
            }

            current = newNode;
        }

        public string? GetCurrentPage()
        {
            return current?.Url;
        }

        public void GoBack()
        {
            if (current?.Previous != null)
            {
                current = current.Previous;
            }
        }

        public void GoForward()
        {
            if (current?.Next != null)
            {
                current = current.Next;
            }
        }
    }
}
