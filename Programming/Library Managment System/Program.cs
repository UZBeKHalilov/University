/*
Task 7: Library Management System

Description: Create a simple library management system where students can borrow and return books. 
Use an array to store the list of available books and a do-while loop to process borrowing and returning operations.

Requirements:

    Store book titles in an array.
    Use if statements to check if a book is available or already borrowed.
    Display the list of borrowed books and available books after each transaction.
 */

namespace Library_Managment_System
{
    internal class Program
    {
        static void Main(string[] args)
        {

            LibrarySystem();
        }

        static void LibrarySystem()
        {
            string[] books = { "Oq kema", "Qora kema", "Sariq kema" };
            bool[] isBorrowed = { true, false, false };

            for (var index = 0; index < books.Length; index++)
            {
                var book = books[index];
                Console.WriteLine($"Book name: {book}");
            }

            for (int i = 0; i < books.Length; i++)
            {
                if (isBorrowed[i])
                {
                    Console.WriteLine($"\n{books[i]} is is borrowed");
                }
                else
                {
                    Console.WriteLine($"\n{books[i]} is allowed!!");
                }
            }

            Console.WriteLine("[1] - Return book");
            Console.WriteLine("[2] - Borrow book");
            Console.Write("[]>");

            short choose = short.Parse(Console.ReadLine());

            if (choose == 1)
            {
                Console.Write("Enter the book name to return: ");
                string bookName = Console.ReadLine();

                int bookIndex = Array.IndexOf(books, bookName);

                if (bookIndex != -1 && isBorrowed[bookIndex])
                {
                    isBorrowed[bookIndex] = false;
                    Console.WriteLine($"{bookName} is returned successfully.");
                }
                else
                {
                    Console.WriteLine($"{bookName} is not borrowed or does not exist.");
                }
            }else if (choose == 2)
            {

                Console.WriteLine("Enter the book name to borrow");
                string bookName = Console.ReadLine();

                int bookIndex = Array.IndexOf(books, bookName);

                if (bookIndex != -1 && !isBorrowed[bookIndex])
                {
                    isBorrowed[bookIndex] = true;
                    Console.WriteLine($"{bookName} is borrowed successfully.");
                }
                else
                {
                    Console.WriteLine($"{bookName} is borrowed or does not exist");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter 1 or 2.");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            LibrarySystem();
        }
    }
}
