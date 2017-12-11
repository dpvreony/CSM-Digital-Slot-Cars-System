
namespace SlotCarsGo.Models
{
    using SlotCarsGo.Models.Manager;
    using SlotCarsGo.Models.Racing;
    using System;
    using System.Collections.Generic;

    class Player
    {
        User user;
        Car car;
        List<TimeSpan> previousLaps;
        TimeSpan currentLap;
        TimeSpan lastLap;

        public Player(User user)
        {
            this.user = user;
            this.car = user.SelectedCar;
            this.previousLaps = new List<TimeSpan>();
            this.currentLap = new TimeSpan();
            this.lastLap = new TimeSpan();
        }

        public Car Car { get => car; set => car = value; }
        public User User { get => user; set => user = value; }
        public List<TimeSpan> PreviousLaps { get => previousLaps; set => previousLaps = value; }
        public TimeSpan CurrentLap { get => currentLap; set => currentLap = value; }
        public TimeSpan LastLap { get => lastLap; set => lastLap = value; }
    }
}
