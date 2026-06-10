using System.ComponentModel.DataAnnotations;

namespace WatApi.DTO.JobOffer
{
    public class JobOfferPublishDto
    {
        [MaxLength(500, ErrorMessage = "Feedback should be less than 500 characters!")]
        public string? Feedback { get; set; }
        public int? Rating { get; set; }
    }
}
