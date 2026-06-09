namespace WatApi.DTO.ImportantInfo
{
    public class ImportantInfoCreateDto
    {
        public string? SevisId { get; set; }
        public DateTime? VisaAppointment { get; set; }
        public DateTime? FlightDate { get; set; }
        public string? Ds160 { get; set; }
        public string? Ds2019 { get; set; }
    }
}
