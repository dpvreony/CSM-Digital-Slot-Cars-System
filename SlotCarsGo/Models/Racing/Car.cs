
using SlotCarsGo.Models.Manager;
using SlotCarsGo.Services;
using System;
using Windows.Storage;

namespace SlotCarsGo.Models.Racing
{
    public class Car
    {
        string carID;
        string name;
        string imageName;
        TimeSpan trackRecord;
        string recordHolderFullName;

        public Car(string carID, string name, string imageName, TimeSpan trackRecord, string recordHolderFullName)
        {
            this.carID = carID;
            this.name = name;
            this.imageName = "/Assets/CarImages/" + imageName;
            this.trackRecord = trackRecord;
            this.recordHolderFullName = recordHolderFullName;
        }

        public string CarID { get => carID; }
        public string Name { get => name; set => name = value; }
        public TimeSpan TrackRecord { get => trackRecord; set => this.trackRecord = value; }
        public String RecordHolder { get => this.recordHolderFullName; set => recordHolderFullName = value; }
        public string ImageName { get => imageName; set => imageName = value; }

        public static readonly Car DefaultCar = new Car("1", "Default Car", "0.png", new TimeSpan(0,0,5), "Default User");
    }
}
