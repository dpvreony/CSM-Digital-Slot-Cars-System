
using SlotCarsGo.Models.Manager;
using SlotCarsGo.Services;
using System;

namespace SlotCarsGo.Models.Racing
{
    public class Car
    {
        uint carID;
        string name;
        string imageName;
        TimeSpan trackRecord;
        string recordHolderFullName;

        public Car(uint carID, string name, string imageName, TimeSpan trackRecord, string recordHolderFullName)
        {
            this.carID = carID;
            this.name = name;
            this.imageName = imageName;
            this.trackRecord = trackRecord;
            this.recordHolderFullName = recordHolderFullName;
        }

        public uint CarID { get => carID; }
        public string Name { get => name; set => name = value; }
        public TimeSpan TrackRecord { get => trackRecord; set => this.trackRecord = value; }
        public String RecordHolder { get => this.recordHolderFullName; set => recordHolderFullName = value; }
        public string ImageName { get => imageName; set => imageName = value; }

        private static Car defaultCar = new Car(0, "Not set", "/Assets/CarImages/0.png", new TimeSpan(0,5,0), "Default");
        public static Car DefaultCar { get => defaultCar; }
    }
}
