namespace WatApi.DTO.ImportantInfo
{
    public class ImportantInfoResponseDto
    {
        public string? SevisId { get; set; }
        public DateTime? VisaAppointment { get; set; }
        public DateTime? FlightDate { get; set; }
        public string? Ds160 { get; set; }
        public DateTime? StartOfWork { get; set; }
        public DateTime? EndOfWork { get; set; }
    }
}
