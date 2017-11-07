
namespace RaceDirectorClientGUI.Models.Racing
{
    abstract class CarBase
    {
        uint carID;
        Player player;

        public CarBase(uint carID)
        {
            this.CarID = carID;
        }

        public uint CarID { get => carID; set => carID = value; }
        internal Player Player { get => player; set => player = value; }
    }
}
