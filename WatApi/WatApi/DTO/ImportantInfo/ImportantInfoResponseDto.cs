namespace WatApi.DTO.ImportantInfo
{
    public class ImportantInfoResponseDto
    {
        public string? SevisId { get; set; }
        public DateOnly? VisaAppointment { get; set; }
        public DateOnly? FlightDate { get; set; }
        public string? Ds160 { get; set; }
        public DateOnly? StartOfWork { get; set; }
        public DateOnly? EndOfWork { get; set; }
    }
}
