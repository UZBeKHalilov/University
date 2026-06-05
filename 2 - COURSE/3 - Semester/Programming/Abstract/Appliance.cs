using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstract
{
    public abstract class Appliance
    {
        public abstract void TurnOn();

        public virtual void TurnOff()
        {
            Console.WriteLine("turned off.");
        }
    }
    public class WashingMachine : Appliance
    {
        
        public override void TurnOn()
        {
            Console.WriteLine("Washing Machine is now turned on. Ready to wash clothes.");
        }
    }

    public class AirConditioner : Appliance
    {
        public override void TurnOn()
        {
            Console.WriteLine("Air Conditioner is now turned on. Cooling the room.");
        }

        public ov void TurnOff()
        {
            Console.WriteLine("turned off the Air Conditioner.");
        }
    }
}
