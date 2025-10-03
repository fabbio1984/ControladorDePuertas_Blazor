namespace DoorController.Models
{
    public class DoorEvent
    {
        public int Id { get; set; }
        public int DoorId { get; set; }
        public string Action { get; set; } = "Close"; // "Open" or "Close"
        public DateTime At { get; set; } = DateTime.UtcNow;

        public Door? Door { get; set; }
    }
}
