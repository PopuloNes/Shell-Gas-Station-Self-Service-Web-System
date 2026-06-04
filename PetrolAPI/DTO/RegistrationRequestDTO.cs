using System.ComponentModel.DataAnnotations;

namespace PetrolAPI.DTO
{
    public class RegistrationRequestDTO
    {
        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Required]
        public required string ClientName { get; set; }
        
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
         
    }
}
