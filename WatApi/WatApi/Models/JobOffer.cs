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
        public JobOfferStatus Status { get; set; } = JobOfferStatus.ReadyToReview;
        public bool HousingProvided { get; set; } = false;
        public decimal? HousingCostPerWeek { get; set; }
        public int Year { get; set; }
    }
}
