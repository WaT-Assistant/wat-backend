using System.ComponentModel.DataAnnotations;

namespace WatApi.DTO
{
    public class UserRegistrationDto
    {
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password should be at least 8 charachters long!")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;
    }
}
