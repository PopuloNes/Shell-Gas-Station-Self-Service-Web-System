using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using gsst.Services;
using gsst.Model.FuelStuff;

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseSqlite("Data Source=gsnetwork.db");
var db = new AppDbContext(optionsBuilder.Options);

var station = db.GasStations.Include(s => s.Pumps).Include(s => s.Tanks).ThenInclude(t => t.ConnectedPumps).FirstOrDefault(s => s.Id == 11);
Console.WriteLine($"Station: {station.Name}");
foreach(var t in station.Tanks) {
    Console.WriteLine($"Tank {t.Id} FuelTypeId: {t.FuelTypeId} ConnectedPumps count: {t.ConnectedPumps.Count}");
}
foreach(var p in station.Pumps) {
    Console.WriteLine($"Pump {p.Id} Status: {p.Status}");
}
