using gsst.Model;
using gsst.Model.FuelStuff;
using gsst.Model.User;
using gsst.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetrolAPI.DTO;
using System.Security.Claims;

namespace PetrolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Manager)]
    public class ManagerStationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ManagerStationController(AppDbContext context)
        {
            _context = context;
        }

        private int? GetManagerGasStationId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "GasStationId");
            if (claim != null && int.TryParse(claim.Value, out int stationId))
            {
                return stationId;
            }
            return null;
        }

        [HttpGet]
        public async Task<ActionResult<GasStationDetailsDTO>> GetMyStation()
        {
            var stationId = GetManagerGasStationId();
            if (stationId == null) return BadRequest(new { message = "Manager is not assigned to any gas station." });

            var station = await _context.GasStations
                .Include(s => s.Pumps)
                .Include(s => s.Tanks)
                    .ThenInclude(t => t.FuelType)
                .Include(s => s.Tanks)
                    .ThenInclude(t => t.ConnectedPumps)
                .FirstOrDefaultAsync(s => s.Id == stationId);

            if (station == null) return NotFound(new { message = "Assigned gas station not found." });

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

        public class ReplenishRequest
        {
            public double Amount { get; set; }
        }

        [HttpPost("tanks/{id}/replenish")]
        public async Task<IActionResult> ReplenishTank(int id, [FromBody] ReplenishRequest request)
        {
            var stationId = GetManagerGasStationId();
            if (stationId == null) return Unauthorized();

            var tank = await _context.Tanks.FirstOrDefaultAsync(t => t.Id == id && t.GasStationId == stationId);
            if (tank == null) return NotFound(new { message = "Tank not found in your station." });

            if (request.Amount <= 0) return BadRequest(new { message = "Amount must be positive." });

            try
            {
                tank.Volume += request.Amount;
                await _context.SaveChangesAsync();
                return Ok(new { message = "Tank replenished successfully.", newVolume = tank.Volume });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        public class PumpStatusRequest
        {
            public int Status { get; set; } // 0 = Disabled, 1 = Maintenance, 2 = Free, etc.
        }

        [HttpPut("pumps/{id}/status")]
        public async Task<IActionResult> UpdatePumpStatus(int id, [FromBody] PumpStatusRequest request)
        {
            var stationId = GetManagerGasStationId();
            if (stationId == null) return Unauthorized();

            var pump = await _context.Pumps.FirstOrDefaultAsync(p => p.Id == id && p.GasStationId == stationId);
            if (pump == null) return NotFound(new { message = "Pump not found in your station." });

            if (!Enum.IsDefined(typeof(PumpStatus), request.Status))
                return BadRequest(new { message = "Invalid pump status." });

            pump.Status = (PumpStatus)request.Status;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Pump status updated.", newStatus = (int)pump.Status });
        }

        public class NewPumpRequest
        {
            public string Name { get; set; } = default!;
        }

        [HttpPost("pumps")]
        public async Task<IActionResult> AddPump([FromBody] NewPumpRequest request)
        {
            var stationId = GetManagerGasStationId();
            if (stationId == null) return Unauthorized();

            if (string.IsNullOrEmpty(request.Name)) return BadRequest(new { message = "Pump name is required." });

            var pump = new Pump
            {
                Name = request.Name,
                GasStationId = stationId.Value,
                Status = PumpStatus.Disabled
            };

            _context.Pumps.Add(pump);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Pump added successfully.", pumpId = pump.Id });
        }

        [HttpDelete("pumps/{id}")]
        public async Task<IActionResult> DeletePump(int id)
        {
            var stationId = GetManagerGasStationId();
            if (stationId == null) return Unauthorized();

            var pump = await _context.Pumps.Include(p => p.ConnectedTanks).FirstOrDefaultAsync(p => p.Id == id && p.GasStationId == stationId);
            if (pump == null) return NotFound(new { message = "Pump not found in your station." });

            pump.ConnectedTanks.Clear(); // Remove connections first
            _context.Pumps.Remove(pump);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Pump deleted successfully." });
        }

        [HttpPost("pumps/{id}/tanks/{tankId}")]
        public async Task<IActionResult> ConnectFuelToPump(int id, int tankId)
        {
            var stationId = GetManagerGasStationId();
            if (stationId == null) return Unauthorized();

            var pump = await _context.Pumps.Include(p => p.ConnectedTanks).FirstOrDefaultAsync(p => p.Id == id && p.GasStationId == stationId);
            if (pump == null) return NotFound(new { message = "Pump not found." });

            var tank = await _context.Tanks.FirstOrDefaultAsync(t => t.Id == tankId && t.GasStationId == stationId);
            if (tank == null) return NotFound(new { message = "Tank not found." });

            if (!pump.ConnectedTanks.Any(t => t.Id == tankId))
            {
                pump.ConnectedTanks.Add(tank);
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Tank connected to pump." });
        }

        [HttpDelete("pumps/{id}/tanks/{tankId}")]
        public async Task<IActionResult> DisconnectFuelFromPump(int id, int tankId)
        {
            var stationId = GetManagerGasStationId();
            if (stationId == null) return Unauthorized();

            var pump = await _context.Pumps.Include(p => p.ConnectedTanks).FirstOrDefaultAsync(p => p.Id == id && p.GasStationId == stationId);
            if (pump == null) return NotFound(new { message = "Pump not found." });

            var tank = pump.ConnectedTanks.FirstOrDefault(t => t.Id == tankId);
            if (tank != null)
            {
                pump.ConnectedTanks.Remove(tank);
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Tank disconnected from pump." });
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactions()
        {
            var stationId = GetManagerGasStationId();
            if (stationId == null) return Unauthorized();

            var orders = await _context.Orders
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .Include(o => o.User)
                .Include(o => o.Pump)
                .Where(o => o.GasStationId == stationId.Value)
                .OrderByDescending(o => o.Date)
                .ToListAsync();

            var userIds = orders.Select(o => (int?)o.UserId).Distinct().ToList();
            var bonusCards = await _context.BonusCards
                .Where(b => userIds.Contains(b.UserId))
                .ToDictionaryAsync(b => b.UserId ?? 0);

            var transactions = orders.Select(o =>
            {
                var item = o.Items.FirstOrDefault();
                var card = bonusCards.GetValueOrDefault(o.UserId);
                return new TransactionDTO
                {
                    OrderId = o.Id,
                    Date = o.Date,
                    PumpName = o.Pump?.Name ?? "Unknown",
                    FuelName = item?.Product?.Name ?? "Unknown",
                    Volume = item?.Quantity ?? 0,
                    Amount = item?.Subtotal ?? 0,
                    ClientPhone = o.User?.PhoneNumber ?? "N/A",
                    ClientBarcode = card?.Barcode ?? "N/A",
                    PaymentCard = string.IsNullOrEmpty(o.PaymentCardNumber) ? "N/A" : o.PaymentCardNumber
                };
            }).ToList();

            return Ok(transactions);
        }
    }
}
