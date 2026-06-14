namespace WatApi.Models
{
    public class ImportantInfo
    {
        public Guid Id { get; set; }
        public JobOffer JobOffer { get; set; } = null!;
        public Guid JobOfferId { get; set; }
        public string? SevisID { get; set; } = string.Empty;
        public DateOnly? VisaAppointment { get; set; }
        public DateOnly? Flight { get; set; }
        public string? DS160 { get; set; } = string.Empty;
        public DateOnly? StartOfWork { get; set; }
        public DateOnly? EndOfWork { get; set; }
    }
}
