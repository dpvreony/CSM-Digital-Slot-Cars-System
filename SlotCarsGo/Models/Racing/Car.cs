
using SlotCarsGo.Models.Manager;
using SlotCarsGo.Services;
using System;

namespace SlotCarsGo.Models.Racing
{
    public class Car
    {
        uint carID;
        string name;
        string imagePath;
        TimeSpan trackRecord;
        int recordHolderId;

        public Car(uint carID, string name, string imagePath, TimeSpan trackRecord, int recordHolderId)
        {
            this.CarID = carID;
            this.Name = name;
            this.ImagePath = imagePath;
            this.TrackRecord = trackRecord;
            this.recordHolderId = recordHolderId;
        }

        public uint CarID { get => carID; set => carID = value; }
        public string Name { get => name; set => name = value; }
        public TimeSpan TrackRecord { get => trackRecord; set => trackRecord = value; }
        public Driver RecordHolder { get => LoggedInUsersDataService.GetUser(recordHolderId); set => recordHolderId = value.Id; }
        public string ImagePath { get => imagePath; set => imagePath = value; }

        private static Car defaultCar = new Car(0, "Not set", "/Assets/CarImages/0.png", new TimeSpan(0,5,0), 0);
        public static Car DefaultCar { get => defaultCar; }
    }
}
