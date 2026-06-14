using System.ComponentModel.DataAnnotations;

namespace WatApi.DTO.ImportantInfo
{
    public class ImportantInfoUpdateDto
    {
        [MaxLength(30, ErrorMessage = "Sevis ID can't exceed 30 characters!")]
        public string? SevisId { get; set; }
        public DateTime? VisaAppointment { get; set; }
        public DateTime? FlightDate { get; set; }
        [MaxLength(30, ErrorMessage = "DS160 number can't exceed 30 characters!")]
        public string? Ds160 { get; set; }
        public DateTime? StartOfWork { get; set; }
        public DateTime? EndOfWork { get; set; }
    }
}
