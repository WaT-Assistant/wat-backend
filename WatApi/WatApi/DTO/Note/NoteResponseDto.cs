namespace WatApi.DTO.Note
{
    public class NoteResponseDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string ColorHex { get; set; } = "#FFFF88";
        public DateTime CreatedAt { get; set; }
    }
}
