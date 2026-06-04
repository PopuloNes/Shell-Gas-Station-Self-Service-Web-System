using gsst.Model;
using gsst.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PetrolAPI.DTO;

namespace PetrolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GasStationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GasStationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GasStationDetailsDTO>>> GetGasStations()
        {
            var stations = await _context.GasStations
                .Include(s => s.Pumps)
                .Include(s => s.Tanks)
                    .ThenInclude(t => t.FuelType)
                .Include(s => s.Tanks)
                    .ThenInclude(t => t.ConnectedPumps)
                .ToListAsync();

            var dtos = stations.Select(station => new GasStationDetailsDTO
            {
                Id = station.Id,
                Name = station.Name,
                Address = station.Address,
                Latitude = station.Latitude,
                Longitude = station.Longitude,
                Pumps = station.Pumps.Select(p => new PumpDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Status = (int)p.Status
                }).ToList(),
                AvailableFuels = station.Tanks.Where(t => t.FuelType != null).Select(t => new FuelOptionDTO
                {
                    TankId = t.Id,
                    FuelTypeId = t.FuelType.Id,
                    FuelName = t.FuelType.Name,
                    Price = t.FuelType.Price,
                    AvailableVolume = t.Volume,
                    PumpIds = t.ConnectedPumps.Select(p => p.Id).ToList()
                }).GroupBy(f => f.FuelTypeId).Select(g => g.First()).ToList() // Unique fuels by type
            });

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GasStation>> GetGasStation(int id)
        {
            var station = await _context.GasStations.FindAsync(id);
            if (station == null) return NotFound();
            return station;
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<GasStationDetailsDTO>> GetGasStationDetails(int id)
        {
            var station = await _context.GasStations
                .Include(s => s.Pumps)
                .Include(s => s.Tanks)
                    .ThenInclude(t => t.FuelType)
                .Include(s => s.Tanks)
                    .ThenInclude(t => t.ConnectedPumps)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (station == null) return NotFound();

            var dto = new GasStationDetailsDTO
            {
                Id = station.Id,
                Name = station.Name,
                Address = station.Address,
                Latitude = station.Latitude,
                Longitude = station.Longitude,
                Pumps = station.Pumps.Select(p => new PumpDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Status = (int)p.Status
                }).ToList(),
                AvailableFuels = station.Tanks.Where(t => t.FuelType != null).Select(t => new FuelOptionDTO
                {
                    TankId = t.Id,
                    FuelTypeId = t.FuelType.Id,
                    FuelName = t.FuelType.Name,
                    Price = t.FuelType.Price,
                    AvailableVolume = t.Volume,
                    PumpIds = t.ConnectedPumps.Select(p => p.Id).ToList()
                }).ToList()
            };

            return Ok(dto);
        }
    }
}
