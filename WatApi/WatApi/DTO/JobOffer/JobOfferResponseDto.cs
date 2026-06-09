using WatApi.Models;

namespace WatApi.DTO.JobOffer
{
    public class JobOfferResponseDto
    {
        public Guid Id { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Employer { get; set; } = string.Empty;
        public string PlaceOfWork { get; set; } = string.Empty;
        public decimal PayPerHour { get; set; }
        public bool HousingProvided { get; set; }
        public decimal? HousingCostPerWeek { get; set; }
        public JobOfferStatus Status { get; set; }
        public int Year { get; set; }
    }
}
