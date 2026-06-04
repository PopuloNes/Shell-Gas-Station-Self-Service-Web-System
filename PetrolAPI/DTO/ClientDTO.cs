using System.ComponentModel.DataAnnotations;

namespace PetrolAPI.DTO
{
    public class ClientDTO
    {
        public int Id { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Role { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Barcode { get; set; } = default!;

        public double BonusBalance { get; set; } = default!;

        public int? GasStationId { get; set; }
    }
}
