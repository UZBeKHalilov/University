using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstract
{
    public abstract class Order
    {
        public abstract void ProcessOrder();
    }
    public class OnlineOrder : Order
    {
        public override void ProcessOrder()
        {
            Console.WriteLine("Online Order");
        }
    }
    public class InStoreOrder : Order
    {
        public override void ProcessOrder()
        {
            Console.WriteLine("In Store Order");
        }
    }
}
