//Task 8: Inventory Tracking System

//Description: Develop an inventory management system where users can add, update, or remove products from an inventory. Use arrays to store product names and quantities, and allow multiple transactions using a do -while loop.

//    Requirements:

//Store product names and their quantities in arrays.
//    Allow users to add new products, update quantities, or remove products.
//    Display the updated inventory list after each operation.


namespace Inventory_Tracking_System
{
    internal class Program
    {
        private static string[] products = new string[10];
        private static int[] quantities = new int[10];
        
        static void Main(string[] args)
        {

            InventoryTrackingSystem();
        }

        private static void InventoryTrackingSystem()
        {
            bool exit = false;
            do
            {
                Console.WriteLine("Welcome to the Inventory Tracking System!");
                Console.WriteLine("1. Add a new product");
                Console.WriteLine("2. Update product quantity");
                Console.WriteLine("3. Remove a product");
                Console.WriteLine("4. Display inventory list");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice (1-5): ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddProduct();
                        break;
                    case 2:
                        UpdateQuantity();
                        break;
                    case 3:
                        RemoveProduct();
                        break;
                    case 4:
                        DisplayInventory();
                        break;
                    case 5:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                        break;
                }
            } while (!exit);
        }

        private static void DisplayInventory()
        {
            throw new NotImplementedException();
        }

        private static void RemoveProduct()
        {
            throw new NotImplementedException();
        }

        private static void UpdateQuantity()
        {
            throw new NotImplementedException();
        }

        private static void AddProduct()
        {

        }
    }
}
