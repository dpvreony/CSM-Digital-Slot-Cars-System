
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
        User recordHolder;

        public Car(uint carID, string name, string imagePath, TimeSpan trackRecord, int recordHolderId)
        {
            this.CarID = carID;
            this.Name = name;
            this.ImagePath = imagePath;
            this.TrackRecord = trackRecord;
            this.RecordHolder = LoggedInUsersDataService.GetUser(recordHolderId);
        }

        public uint CarID { get => carID; set => carID = value; }
        public string Name { get => name; set => name = value; }
        public TimeSpan TrackRecord { get => trackRecord; set => trackRecord = value; }
        public User RecordHolder { get => recordHolder; set => recordHolder = value; }
        public string ImagePath { get => imagePath; set => imagePath = value; }
    }
}
