using System.Collections.Generic;

namespace PetrolAPI.DTO
{
    public class GasStationDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<PumpDTO> Pumps { get; set; } = new List<PumpDTO>();
        public List<FuelOptionDTO> AvailableFuels { get; set; } = new List<FuelOptionDTO>();
    }

    public class PumpDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }

    public class FuelOptionDTO
    {
        public int TankId { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelName { get; set; }
        public double Price { get; set; }
        public double AvailableVolume { get; set; }
        public List<int> PumpIds { get; set; } = new List<int>();
    }
}
