using System.ComponentModel.DataAnnotations;

namespace DoorController.Models
{
    public class Door
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public bool IsOpen { get; set; }

        public DateTime? LastChangedAt { get; set; }
    }
}
