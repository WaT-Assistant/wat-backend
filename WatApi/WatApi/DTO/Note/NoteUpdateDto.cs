using System.ComponentModel.DataAnnotations;

namespace WatApi.DTO.Note
{
    public class NoteUpdateDto
    {
        [Required(ErrorMessage = "Note text is required")]
        [MaxLength(300, ErrorMessage = "Note can't contain more than 300 characters!")]
        public string Text { get; set; } = string.Empty;
        [MaxLength(10)]
        public string ColorHex { get; set; } = "#FFFF88";
    }
}
