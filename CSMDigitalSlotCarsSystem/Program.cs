using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSMDigitalSlotCarsSystem.Models.Comms;
using static CSMDigitalSlotCarsSystem.Enums;

namespace CSMDigitalSlotCarsSystem
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}: Starting 'CSM Digital Slot Cars System'");
            //            Powerbase powerbase = new Powerbase();
            List<Player> players = new List<Player> { new Player() };
            RaceTypeBase raceType = new FreePlayRace(5, new TimeSpan(0,2,0), true);
            RaceSession raceSession = new RaceSession(1, raceType, players);
            Powerbase pb = new Powerbase();
            pb.Run(raceSession);
            while (true)
            {
                Thread.Sleep(60 * 1000);
                System.Diagnostics.Debug.WriteLine($"Bytes on Port: Read: {pb.Port.BytesToRead} Write: {pb.Port.BytesToWrite}");
                pb.CancelPowerbaseDataFlow();
                Thread.Sleep(15 * 1000);
                pb.Run(raceSession);
            }
            //            Task powerbaseTask = new Task(() => powerbase.Run());
            //            powerbaseTask.Start();
            //            powerbaseTask.Wait();
        }
    }
}
