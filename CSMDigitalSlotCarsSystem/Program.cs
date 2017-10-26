using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSMDigitalSlotCarsSystem.Models.Comms;

namespace CSMDigitalSlotCarsSystem
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}: Starting 'CSM Digital Slot Cars System'");
            Powerbase powerbase = new Powerbase();
            powerbase.Run();
            while (!powerbase.PowerbaseRunCancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(60 * 1000);
                System.Diagnostics.Debug.WriteLine($"Bytes on Port: Read: {powerbase.Port.BytesToRead} Write: {powerbase.Port.BytesToWrite}");
            }
            //            Task powerbaseTask = new Task(() => powerbase.Run());
            //            powerbaseTask.Start();
            //            powerbaseTask.Wait();
        }
    }
}
