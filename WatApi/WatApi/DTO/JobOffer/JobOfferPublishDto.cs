using System.ComponentModel.DataAnnotations;

namespace WatApi.DTO.JobOffer
{
    public class JobOfferPublishDto
    {
        [MaxLength(300, ErrorMessage = "Feedback should be less than 300 characters!")]
        public string? Feedback { get; set; }
        public int? Rating { get; set; }
    }
}
