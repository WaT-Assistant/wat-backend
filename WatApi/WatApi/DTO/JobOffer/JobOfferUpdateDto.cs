using System.ComponentModel.DataAnnotations;
using WatApi.Models;

namespace WatApi.DTO.JobOffer
{
    public class JobOfferUpdateDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Position should be less than 50 characters!")]
        public string Position { get; set; } = string.Empty;
        [Required]
        [MaxLength(50, ErrorMessage = "Employer should be less than 50 characters!")]
        public string Employer { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage = "Place of work should be less than 100 characters!")]
        public string PlaceOfWork { get; set; } = string.Empty;
        [Required]
        public decimal PayPerHour { get; set; }
        [Required]
        public bool HousingProvided { get; set; }
        [Required]
        public decimal? HousingCostPerWeek { get; set; }
        [Required]
        public JobOfferStatus Status { get; set; }
        [Required]
        public int Year { get; set; }
    }
}
