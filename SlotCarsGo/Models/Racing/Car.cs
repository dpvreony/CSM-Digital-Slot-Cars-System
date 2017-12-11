
namespace SlotCarsGo.Models.Racing
{
    public class Car
    {
        uint carID;
        string name;

        public Car(uint carID, string name)
        {
            this.CarID = carID;
            this.name = name;
        }

        public uint CarID { get => carID; set => carID = value; }
        public string Name { get => name; set => name = value; }
    }
}
