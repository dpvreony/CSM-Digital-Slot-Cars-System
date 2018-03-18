
using SlotCarsGo.Models.Manager;
using SlotCarsGo.Services;
using System;
using Windows.Storage;

namespace SlotCarsGo.Models.Racing
{
    public class Car
    {
        public Car()
        {
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public TimeSpan TrackRecord { get; set; }
        public string RecordHolder { get; set; }
        public string ImageName { get; set; }

        public static readonly Car DefaultCar = new Car() { Id = "1", Name = "Default Car", ImageName = "0.png", TrackRecord = new TimeSpan(0, 0, 5), RecordHolder = "Default User" };
    }
}
