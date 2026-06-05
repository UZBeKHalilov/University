
namespace BankQueue
{
    internal class Program
    {
        /**
         * 1)
         * Servisini tanlaydi
         * Ismi va yoshini kiritadi
         * Unga nechanchi ro'yxatda ekanligi ko'rsatiladi
         * 
         * 2)
         * Admin Panelga kiradi
         * u yerda barcha servicelarninch barcha ro'yxati ko'rsatiladi
         * keragini o'chiradi yoki qo'shadi
         */
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bank Services");

            Console.WriteLine("Please select a service:");


            SelectBankService();
        }

        private static void SelectBankService()
        {
            Console.WriteLine("1. Registration Account");
            Console.WriteLine("2. Payment");
            Console.WriteLine("3. Customer Support");
            Console.WriteLine("3. Admin Panel");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("You selected Registration Account.");
                    // Add logic for Registration Account
                    break;
                case "2":   
                    Console.WriteLine("You selected Payment.");
                    // Add logic for Payment
                    break;
                case "3":
                    Console.WriteLine("You selected Customer Support.");
                    // Add logic for customer support
                    break;
                case "4":
                    Console.WriteLine("You selected Admin Panel.");
                    // Add logic for Admin Panel
                    break;
                case "5":
                    Console.WriteLine("Thank you for visiting. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    SelectBankService();
                    break;
            }
        }
    }
}
