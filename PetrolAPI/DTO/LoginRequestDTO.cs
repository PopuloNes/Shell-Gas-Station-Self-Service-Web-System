using System.ComponentModel.DataAnnotations;

namespace PetrolAPI.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
