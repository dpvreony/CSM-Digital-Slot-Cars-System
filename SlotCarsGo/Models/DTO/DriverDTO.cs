namespace SlotCarsGo_Server.Models.DTO
{
    public class DriverDTO
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarSource { get; set; }
        public int ControllerId { get; set; }
        internal CarDTO SelectedCar { get; set; }
    }
}
