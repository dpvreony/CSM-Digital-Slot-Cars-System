namespace SlotCarsGo_Server.Models.DTO
{
    public class DriverDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ImageName { get; set; }
        public int ControllerId { get; set; }
        public CarDTO SelectedCar { get; set; }
    }
}
