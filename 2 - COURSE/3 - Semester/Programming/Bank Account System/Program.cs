/*
 Task 5: Bank Account System

Description: Create a banking system where users can deposit and withdraw money from their accounts. Use arrays to store account balances 
and a do-while loop to process multiple transactions.

Requirements:

    Store account balances in an array.
    Use if statements to validate withdrawal amounts and check if sufficient funds are available.
    Allow multiple transactions until the user exits the system.
*/

/*
Задача 5: Система банковских счетов

Описание: Создайте банковскую систему, в которой пользователи могут вносить и снимать деньги со своих счетов. Используйте массивы для хранения остатков на счетах
и цикл do-while для обработки нескольких транзакций.

Требования:

Храните остатки на счетах в массиве.
Используйте операторы if для проверки сумм снятия и проверки наличия достаточных средств.

Разрешите несколько транзакций, пока пользователь не выйдет из системы.
*/

namespace Bank_Account_System
{
    internal class Program
    {
        public static double[] money = { 130.9, 50.6, 13.56 };
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bank!");

            BankAccount();
        }

        static void BankAccount()
        {
            Console.WriteLine($"You have ");

            for (int i = 0; i < money.Length; i++)
            {
                Console.WriteLine($"{i+1} - card : {money[i]}$");
            }

            Console.WriteLine("Choose functions:" +
                "\n[1] Addmoney" +
                "\n[2] Subtract money" +
                "\n[3] Trasfer money");

            switch (Console.ReadLine())
            {
                case "1":
                    AddMoney();

                    break;
                case "2":
                    SubtractMoney();
                   
                    break;
                case "3":
                    TransferMoney();

                    break;
                case "0":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("error");
                    break;
            }

            BankAccount();
        }

        static void AddMoney()
        {
            Console.Write("Choose card number: ");
            short from = short.Parse(Console.ReadLine());
            from--;
            Console.WriteLine($"money in this card {money[from]}$");

            Console.WriteLine("How much u wanna add?");

            double addingMoney = double.Parse(Console.ReadLine());

            money[from] = money[from] + addingMoney;
            Console.WriteLine($"now in this card {money[from]}$");
        }
        static void SubtractMoney()
        {
            Console.Write("Choose card number: ");

            short from = short.Parse(Console.ReadLine());
            from--;

            Console.WriteLine($"money in this card {money[from]}$");

            Console.WriteLine("How much u wanna subtract?");

            double subtractingMoney = double.Parse(Console.ReadLine());

            money[from] = money[from] - subtractingMoney;
            Console.WriteLine($"now in this card {money[from]}$");
        }
        static void TransferMoney()
        {
            Console.Write("Choose card number: ");
            short from = short.Parse(Console.ReadLine());
            from--;

            Console.WriteLine("Choose card number to transfer");
            short to = short.Parse(Console.ReadLine());
            to--;

            Console.WriteLine("How much?");
            double needTarnsfer = double.Parse(Console.ReadLine());

            money[from] = money[from] - needTarnsfer;
            money[to] = money[to] + needTarnsfer;
        }

    }
}
