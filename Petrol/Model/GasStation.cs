using System;
using System.Collections.Generic;

namespace gsst.Model
{
    public class GasStation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<gsst.Model.FuelStuff.Tank> Tanks { get; set; } = new List<gsst.Model.FuelStuff.Tank>();
        public ICollection<gsst.Model.FuelStuff.Pump> Pumps { get; set; } = new List<gsst.Model.FuelStuff.Pump>();
        public ICollection<gsst.Model.User.User> Managers { get; set; } = new List<gsst.Model.User.User>();
    }
}
