using System;

namespace SlotCarsGo_Server.Models.DTO
{
    public class CarDTO
    {
        uint carID;
        string name;
        string imagePath;
        TimeSpan trackRecord;
        string recordHolderFullName;

        public CarDTO(uint carID, string name, string imagePath, TimeSpan trackRecord, string recordHolderFullName)
        {
            this.carID = carID;
            this.name = name;
            this.imagePath = imagePath;
            this.trackRecord = trackRecord;
            this.recordHolderFullName = recordHolderFullName;
        }

        public uint CarID { get => carID; }
        public string Name { get => name; set => name = value; }
        public TimeSpan TrackRecord { get => trackRecord; set => this.trackRecord = value; }
        public String RecordHolder { get => this.recordHolderFullName; set => recordHolderFullName = value; }
        public string ImagePath { get => imagePath; set => imagePath = value; }
    }
}
