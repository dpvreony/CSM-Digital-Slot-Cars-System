using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSMDigitalSlotCarsSystem
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}: Starting 'CSM Digital Slot Cars System'");

            Powerbase powerbase = new Powerbase();
            powerbase.Run();
        }
    }
}
