using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstract
{
    public abstract class Account
    {
        public double Balance {get; protected set;}

        public Account(double ammount)
        {
            Balance = ammount;
        }
        public abstract void Withdraw(double amount);
    }

    public class SavingsAccount : Account
    {
        public SavingsAccount(double ammount) : base(ammount)
        {
        }


        public override void Withdraw(double amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Withdrawal amount must be greater than zero.");
                return;
            }

            if (Balance - amount < 100)
            {
                Console.WriteLine("Withdrawal denied! Balance cannot go below $100.");
            }
            else
            {
                Balance -= amount;
                Console.WriteLine($"Withdrawal successful! Your new balance is: ${Balance}");
            }
        }
    }
}
