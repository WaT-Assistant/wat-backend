namespace WatApi.Models
{
    public class JobOffer
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string Position { get; set; } = string.Empty;
        public string Employer { get; set; } = string.Empty;
        public string PlaceOfWork {  get; set; } = string.Empty;
        public decimal PayPerHour { get; set; }
        public bool HousingProvided { get; set; } = false;
        public decimal? HousingCostPerWeek { get; set; }
        public int Year { get; set; }
        public bool IsPublished { get; set; } = false;
        public string? Feedback { get; set; }
        public int? Rating { get; set; }
        public ImportantInfo? ImportantInfo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
