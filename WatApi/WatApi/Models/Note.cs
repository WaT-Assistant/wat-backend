using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace WatApi.Models
{
    public class Note
    {
        public Guid Id { get; set; }
        public User? User { get; set; }
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(300, ErrorMessage = "Note can't contain more than 300 characters!")]
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [MaxLength(10)]
        public string ColorHex { get; set; } = "#FFFF88";

    }
}
