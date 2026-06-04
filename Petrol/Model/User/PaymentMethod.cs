using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gsst.Model.User
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string Type { get; set; } // "Visa/Mastercard", "Blik", "Crypto"

        [Required]
        public string Details { get; set; } // Card number, Blik phone number, or Crypto wallet

        public bool IsDefault { get; set; }
    }
}
