using gsst.Model;
using gsst.Model.FuelStuff;
using gsst.Model.User;
using Microsoft.EntityFrameworkCore;

namespace gsst.Services
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<User> Users { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<BonusCard> BonusCards { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Good> Goods { get; set; }

        public DbSet<Pump> Pumps { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<GasStation> GasStations { get; set; }

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FullName = "Михайло Стадніков", Username = "admin", Password = "admin", PhoneNumber = "+48000000001", Role = UserRoles.Admin },
                new User { Id = 2, FullName = "Олексій Романенко", Username = "cashier", Password = "cashier", Role = UserRoles.Cashier },
                new User { Id = 3, FullName = "Manager Station 1", Username = "manager1", Password = "manager", Role = UserRoles.Manager, GasStationId = 1 },
                new User { Id = 4, FullName = "Manager Station 2", Username = "manager2", Password = "manager", Role = UserRoles.Manager, GasStationId = 2 },
                new User { Id = 5, FullName = "Manager Station 3", Username = "manager3", Password = "manager", Role = UserRoles.Manager, GasStationId = 3 },
                new User { Id = 6, FullName = "Manager Station 4", Username = "manager4", Password = "manager", Role = UserRoles.Manager, GasStationId = 4 },
                new User { Id = 7, FullName = "Manager Station 5", Username = "manager5", Password = "manager", Role = UserRoles.Manager, GasStationId = 5 },
                new User { Id = 8, FullName = "Manager Station 6", Username = "manager6", Password = "manager", Role = UserRoles.Manager, GasStationId = 6 },
                new User { Id = 9, FullName = "Manager Station 7", Username = "manager7", Password = "manager", Role = UserRoles.Manager, GasStationId = 7 },
                new User { Id = 10, FullName = "Manager Station 8", Username = "manager8", Password = "manager", Role = UserRoles.Manager, GasStationId = 8 },
                new User { Id = 11, FullName = "Manager Station 9", Username = "manager9", Password = "manager", Role = UserRoles.Manager, GasStationId = 9 },
                new User { Id = 12, FullName = "Manager Station 10", Username = "manager10", Password = "manager", Role = UserRoles.Manager, GasStationId = 10 },
                new User { Id = 13, FullName = "Manager Station 11", Username = "manager11", Password = "manager", Role = UserRoles.Manager, GasStationId = 11 },
                new User { Id = 14, FullName = "Manager Station 12", Username = "manager12", Password = "manager", Role = UserRoles.Manager, GasStationId = 12 }
            );

            modelBuilder.Entity<GasStation>().HasData(
                new GasStation { Id = 1, Name = "Shell Warszawa Centrum", Address = "Złota 59, 00-120 Warszawa", Latitude = 52.2300, Longitude = 21.0024 },
                new GasStation { Id = 2, Name = "Shell Warszawa Praga", Address = "Targowa 12, 03-736 Warszawa", Latitude = 52.2514, Longitude = 21.0416 },
                new GasStation { Id = 3, Name = "Orlen Warszawa Wola", Address = "Wolska 15, 01-201 Warszawa", Latitude = 52.2335, Longitude = 20.9702 },
                new GasStation { Id = 4, Name = "BP Warszawa Mokotów", Address = "Puławska 427, 02-801 Warszawa", Latitude = 52.1481, Longitude = 21.0205 },
                new GasStation { Id = 5, Name = "Circle K Warszawa Ursynów", Address = "KEN 98, 02-777 Warszawa", Latitude = 52.1528, Longitude = 21.0450 },
                new GasStation { Id = 6, Name = "Lotos Warszawa Bielany", Address = "Kasprowicza 117, 01-949 Warszawa", Latitude = 52.2859, Longitude = 20.9388 },
                new GasStation { Id = 7, Name = "Shell Warszawa Ochota", Address = "Grójecka 100, 02-389 Warszawa", Latitude = 52.2104, Longitude = 20.9750 },
                new GasStation { Id = 8, Name = "Orlen Warszawa Żoliborz", Address = "Mickiewicza 20, 01-517 Warszawa", Latitude = 52.2682, Longitude = 20.9856 },
                new GasStation { Id = 9, Name = "Amic Warszawa Targówek", Address = "Radzyminska 200, 03-674 Warszawa", Latitude = 52.2741, Longitude = 21.0743 },
                new GasStation { Id = 10, Name = "Moya Warszawa Wawer", Address = "Patriotów 100, 04-944 Warszawa", Latitude = 52.1802, Longitude = 21.1895 },
                new GasStation { Id = 11, Name = "Circle K Warszawa Włochy", Address = "Krakowska 200, 02-219 Warszawa", Latitude = 52.1834, Longitude = 20.9416 },
                new GasStation { Id = 12, Name = "BP Warszawa Bemowo", Address = "Górczewska 212, 01-460 Warszawa", Latitude = 52.2393, Longitude = 20.9165 }
            );

            modelBuilder.Entity<FuelType>().HasData(
                new FuelType { Id = 1, Name = "A-95", Price = 5.0 },
                new FuelType { Id = 2, Name = "A-92", Price = 4.0 },
                new FuelType { Id = 3, Name = "Diesel", Price = 6.0 },
                new FuelType { Id = 4, Name = "Gas", Price = 3.0 }
            );

            var pumps = new List<Pump>();
            // Station 1 & 2
            pumps.Add(new Pump { Id = 1, Name = "Pump 1", GasStationId = 1 });
            pumps.Add(new Pump { Id = 2, Name = "Pump 2", GasStationId = 1 });
            pumps.Add(new Pump { Id = 3, Name = "Pump 3", GasStationId = 2 });
            pumps.Add(new Pump { Id = 4, Name = "Pump 4", GasStationId = 2 });
            
            // New stations 3-12 (4 pumps each)
            int pumpId = 5;
            for (int stationId = 3; stationId <= 12; stationId++)
            {
                for (int p = 1; p <= 4; p++)
                {
                    pumps.Add(new Pump { Id = pumpId, Name = $"Pump {p}", GasStationId = stationId });
                    pumpId++;
                }
            }
            modelBuilder.Entity<Pump>().HasData(pumps);

            // Define Tanks manually or programmatically. C# anonymous types need exact property names.
            var tanks = new List<object>
            {
                new { Id = 1, Capacity = 10000.0, Volume = 5000.0, FuelTypeId = 1, GasStationId = 1 },
                new { Id = 2, Capacity = 10000.0, Volume = 5000.0, FuelTypeId = 2, GasStationId = 1 },
                new { Id = 3, Capacity = 10000.0, Volume = 5000.0, FuelTypeId = 3, GasStationId = 2 },
                new { Id = 4, Capacity = 10000.0, Volume = 5000.0, FuelTypeId = 4, GasStationId = 2 }
            };

            var pumpTanks = new List<object>
            {
                new { ConnectedPumpsId = 1, ConnectedTanksId = 1 },
                new { ConnectedPumpsId = 1, ConnectedTanksId = 2 },
                new { ConnectedPumpsId = 2, ConnectedTanksId = 1 },
                new { ConnectedPumpsId = 3, ConnectedTanksId = 3 },
                new { ConnectedPumpsId = 3, ConnectedTanksId = 4 },
                new { ConnectedPumpsId = 4, ConnectedTanksId = 4 }
            };

            int tankId = 5;
            
            void AddTanksForStation(int stationId, int[] fuelTypes)
            {
                var stationTankIds = new List<int>();
                foreach (var fuelType in fuelTypes)
                {
                    tanks.Add(new { Id = tankId, Capacity = 10000.0, Volume = 5000.0, FuelTypeId = fuelType, GasStationId = stationId });
                    stationTankIds.Add(tankId);
                    tankId++;
                }
                
                // Connect all 4 pumps of this station to all its tanks
                int startPumpId = (stationId - 3) * 4 + 5;
                for (int i = 0; i < 4; i++)
                {
                    int currentPump = startPumpId + i;
                    foreach (var tid in stationTankIds)
                    {
                        pumpTanks.Add(new { ConnectedPumpsId = currentPump, ConnectedTanksId = tid });
                    }
                }
            }

            AddTanksForStation(3, new[] { 1, 2, 3, 4 }); // 4 fuels
            AddTanksForStation(4, new[] { 1, 3 });       // 2 fuels
            AddTanksForStation(5, new[] { 1, 3, 4 });    // 3 fuels
            AddTanksForStation(6, new[] { 1, 2, 3, 4 }); // 4 fuels
            AddTanksForStation(7, new[] { 1, 4 });       // 2 fuels
            AddTanksForStation(8, new[] { 3 });          // 1 fuel
            AddTanksForStation(9, new[] { 1, 2, 3, 4 }); // 4 fuels
            AddTanksForStation(10, new[] { 2, 3, 4 });   // 3 fuels
            AddTanksForStation(11, new[] { 1, 2, 3, 4 });// 4 fuels
            AddTanksForStation(12, new[] { 1, 2 });      // 2 fuels

            modelBuilder.Entity<Tank>().HasData(tanks.ToArray());
            modelBuilder.Entity("PumpTank").HasData(pumpTanks.ToArray());

            modelBuilder.Entity<BonusCard>().HasData(
                new BonusCard { Id = 1, Barcode = "1234", BonusBalance = 100, ClientName = "John Doe" },
                new BonusCard { Id = 2, Barcode = "5678", BonusBalance = 200, ClientName = "Jane Smith" }
            );

        }
    }
}