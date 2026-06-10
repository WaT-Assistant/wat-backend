namespace WatApi.Models
{
    public class ImportantInfo
    {
        public Guid Id { get; set; }
        public JobOffer JobOffer { get; set; } = null!;
        public Guid JobOfferId { get; set; }
        public string? SevisID { get; set; } = string.Empty;
        public DateTime? VisaAppointment { get; set; }
        public DateTime? Flight { get; set; }
        public string? DS160 { get; set; } = string.Empty;
        public string? DS2019 { get; set; } = string.Empty;
    }
}
