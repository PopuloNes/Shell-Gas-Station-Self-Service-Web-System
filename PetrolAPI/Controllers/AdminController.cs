using gsst.Model;
using gsst.Model.User;
using gsst.Model.FuelStuff;
using gsst.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetrolAPI.DTO;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace PetrolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // --- GAS STATIONS CRUD ---

        public class GasStationAdminDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = default!;
            public string Address { get; set; } = default!;
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public List<UserDTO> Managers { get; set; } = new List<UserDTO>();
        }

        [HttpGet("gasstations")]
        public async Task<ActionResult<IEnumerable<GasStationAdminDTO>>> GetGasStations()
        {
            var stations = await _context.GasStations.ToListAsync();
            var managers = await _context.Users.Where(u => u.Role == UserRoles.Manager).ToListAsync();

            var dtos = stations.Select(s => new GasStationAdminDTO
            {
                Id = s.Id,
                Name = s.Name,
                Address = s.Address,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Managers = managers.Where(m => m.GasStationId == s.Id).Select(u => new UserDTO
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Username = u.Username,
                    Role = u.Role,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email,
                    GasStationId = u.GasStationId
                }).ToList()
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("gasstations/{id}/details")]
        public async Task<ActionResult<GasStationDetailsDTO>> GetGasStationDetails(int id)
        {
            var station = await _context.GasStations
                .Include(s => s.Pumps)
                .Include(s => s.Tanks)
                    .ThenInclude(t => t.FuelType)
                .Include(s => s.Tanks)
                    .ThenInclude(t => t.ConnectedPumps)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (station == null) return NotFound(new { message = "Gas station not found" });

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

        public class CreateGasStationRequest
        {
            public string Name { get; set; } = default!;
            public string Address { get; set; } = default!;
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        [HttpPost("gasstations")]
        public async Task<IActionResult> CreateGasStation([FromBody] CreateGasStationRequest request)
        {
            var station = new GasStation
            {
                Name = request.Name,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            _context.GasStations.Add(station);
            await _context.SaveChangesAsync();

            return Ok(station);
        }

        [HttpPut("gasstations/{id}")]
        public async Task<IActionResult> UpdateGasStation(int id, [FromBody] CreateGasStationRequest request)
        {
            var station = await _context.GasStations.FindAsync(id);
            if (station == null) return NotFound(new { message = "Gas station not found" });

            station.Name = request.Name;
            station.Address = request.Address;
            station.Latitude = request.Latitude;
            station.Longitude = request.Longitude;

            await _context.SaveChangesAsync();
            return Ok(station);
        }

        [HttpDelete("gasstations/{id}")]
        public async Task<IActionResult> DeleteGasStation(int id)
        {
            var station = await _context.GasStations.FindAsync(id);
            if (station == null) return NotFound(new { message = "Gas station not found" });

            _context.GasStations.Remove(station);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Gas station deleted successfully" });
        }

        // --- FUEL PRICES ---
        public class UpdateFuelPriceRequest
        {
            public double Price { get; set; }
        }

        public class TogglePumpsRequest
        {
            public int Status { get; set; }
        }

        [HttpPost("gasstations/toggle-pumps")]
        public async Task<IActionResult> ToggleAllPumps([FromBody] TogglePumpsRequest request)
        {
            if (!Enum.IsDefined(typeof(PumpStatus), request.Status))
                return BadRequest(new { message = "Invalid pump status." });

            var pumps = await _context.Pumps.ToListAsync();
            foreach (var pump in pumps)
            {
                pump.Status = (PumpStatus)request.Status;
            }
            await _context.SaveChangesAsync();

            return Ok(new { message = "All pumps status updated." });
        }

        [HttpPut("fueltypes/{id}/price")]
        public async Task<IActionResult> UpdateFuelPrice(int id, [FromBody] UpdateFuelPriceRequest request)
        {
            if (request.Price < 0) return BadRequest(new { message = "Price cannot be negative" });

            var fuelType = await _context.FuelTypes.FindAsync(id);
            if (fuelType == null) return NotFound(new { message = "Fuel type not found" });

            fuelType.Price = request.Price;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Price updated", newPrice = fuelType.Price });
        }

        [HttpGet("fueltypes")]
        public async Task<IActionResult> GetFuelTypes()
        {
            var fuelTypes = await _context.FuelTypes.Where(f => !f.IsDeleted).ToListAsync();
            return Ok(fuelTypes.Select(f => new { f.Id, f.Name, f.Price }));
        }

        public class CreateFuelTypeRequest
        {
            public string Name { get; set; } = default!;
            public double Price { get; set; }
        }

        [HttpPost("fueltypes")]
        public async Task<IActionResult> CreateFuelType([FromBody] CreateFuelTypeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest(new { message = "Name is required" });
            if (request.Price < 0) return BadRequest(new { message = "Price cannot be negative" });

            var fuelType = new FuelType
            {
                Name = request.Name,
                Price = request.Price
            };

            _context.FuelTypes.Add(fuelType);
            await _context.SaveChangesAsync();

            return Ok(new { fuelType.Id, fuelType.Name, fuelType.Price });
        }

        [HttpDelete("fueltypes/{id}")]
        public async Task<IActionResult> DeleteFuelType(int id)
        {
            var fuelType = await _context.FuelTypes.FindAsync(id);
            if (fuelType == null) return NotFound(new { message = "Fuel type not found" });

            fuelType.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Fuel type deleted successfully" });
        }

        // --- USERS CRUD ---

        public class UserDTO
        {
            public int Id { get; set; }
            public string FullName { get; set; } = default!;
            public string Username { get; set; } = default!;
            public string Role { get; set; } = default!;
            public string? PhoneNumber { get; set; }
            public string? Email { get; set; }
            public int? GasStationId { get; set; }
            public string? BonusCardBarcode { get; set; }
            public double? BonusBalance { get; set; }
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var bonusCardsList = await _context.BonusCards.ToListAsync();
            var bonusCards = bonusCardsList
                .Where(b => b.UserId.HasValue)
                .GroupBy(b => b.UserId.Value)
                .ToDictionary(g => g.Key, g => g.First());

            return Ok(users.Select(u => 
            {
                var card = bonusCards.GetValueOrDefault(u.Id);
                return new UserDTO
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Username = u.Username,
                    Role = u.Role,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email,
                    GasStationId = u.GasStationId,
                    BonusCardBarcode = card?.Barcode,
                    BonusBalance = card?.BonusBalance
                };
            }));
        }

        public class CreateUserRequest
        {
            public string FullName { get; set; } = default!;
            public string Username { get; set; } = default!;
            public string Password { get; set; } = default!;
            public string Role { get; set; } = default!;
            public string? PhoneNumber { get; set; }
            public string? Email { get; set; }
            public int? GasStationId { get; set; }
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (_context.Users.Any(u => u.Username == request.Username))
                return BadRequest(new { message = "Username already exists." });

            try
            {
                var user = new User
                {
                    FullName = request.FullName,
                    Username = request.Username,
                    Password = request.Password,
                    Role = request.Role,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    GasStationId = request.Role == UserRoles.Manager ? request.GasStationId : null
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                string? barcode = null;
                double? balance = null;

                if (user.Role == UserRoles.Client)
                {
                    barcode = "CARD" + new Random().Next(100000, 999999).ToString();
                    balance = 0;
                    var bonusCard = new gsst.Model.BonusCard
                    {
                        UserId = user.Id,
                        ClientName = user.FullName,
                        Barcode = barcode,
                        BonusBalance = balance.Value
                    };
                    _context.BonusCards.Add(bonusCard);
                    await _context.SaveChangesAsync();
                }

                return Ok(new UserDTO
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Username = user.Username,
                    Role = user.Role,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    GasStationId = user.GasStationId,
                    BonusCardBarcode = barcode,
                    BonusBalance = balance
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        public class UpdateUserRequest
        {
            public string FullName { get; set; } = default!;
            public string Username { get; set; } = default!;
            public string Role { get; set; } = default!;
            public string? PhoneNumber { get; set; }
            public string? Email { get; set; }
            public int? GasStationId { get; set; }
            public string? Password { get; set; }
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User not found" });

            if (user.Username != request.Username && _context.Users.Any(u => u.Username == request.Username))
                return BadRequest(new { message = "Username already exists." });

            try
            {
                user.FullName = request.FullName;
                user.Username = request.Username;
                user.Role = request.Role;
                user.PhoneNumber = request.PhoneNumber;
                user.Email = request.Email;
                user.GasStationId = request.Role == UserRoles.Manager ? request.GasStationId : null;

                if (!string.IsNullOrEmpty(request.Password))
                {
                    user.Password = request.Password;
                }

                await _context.SaveChangesAsync();

                var card = await _context.BonusCards.FirstOrDefaultAsync(b => b.UserId == user.Id);

                return Ok(new UserDTO
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Username = user.Username,
                    Role = user.Role,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    GasStationId = user.GasStationId,
                    BonusCardBarcode = card?.Barcode,
                    BonusBalance = card?.BonusBalance
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var currentUserIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdStr != null && currentUserIdStr == id.ToString())
            {
                return BadRequest(new { message = "You cannot delete your own admin account." });
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User not found" });

            var bonusCard = await _context.BonusCards.FirstOrDefaultAsync(b => b.UserId == id);
            if (bonusCard != null)
            {
                _context.BonusCards.Remove(bonusCard);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User deleted successfully" });
        }

        // --- GLOBAL STATISTICS ---

        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactions([FromQuery] int? stationId)
        {
            var query = _context.Orders
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .Include(o => o.User)
                .Include(o => o.Pump)
                .AsQueryable();

            if (stationId.HasValue)
            {
                query = query.Where(o => o.GasStationId == stationId.Value);
            }

            var orders = await query.OrderByDescending(o => o.Date).ToListAsync();

            var userIds = orders.Select(o => (int?)o.UserId).Distinct().ToList();
            var bonusCardsList = await _context.BonusCards
                .Where(b => b.UserId.HasValue && userIds.Contains(b.UserId))
                .ToListAsync();
            var bonusCards = bonusCardsList
                .GroupBy(b => b.UserId.Value)
                .ToDictionary(g => g.Key, g => g.First());

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
