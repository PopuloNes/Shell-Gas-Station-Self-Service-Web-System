using gsst.Model;

using gsst.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PetrolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("station/{gasStationId}")]
        public async Task<IActionResult> GetOrdersByStation(int gasStationId)
        {
            var orders = await _context.Orders
                .Where(o => o.GasStationId == gasStationId)
                .ToListAsync();
            return Ok(orders);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.Date)
                .ToListAsync();
                
            var result = orders.Select(o => new {
                OrderId = o.Id,
                Date = o.Date,
                TotalPaid = o.Total,
                BonusesEarned = o.AccruedBonuses,
                BonusSpent = o.BonusSpent,
                PaymentMethod = string.IsNullOrEmpty(o.PaymentCardNumber) ? "Other" : $"Card (***{(o.PaymentCardNumber.Length >= 4 ? o.PaymentCardNumber[^4..] : o.PaymentCardNumber)})",
                GasStationId = o.GasStationId,
                Volume = o.Items.FirstOrDefault()?.Quantity ?? 0,
                FuelName = o.Items.FirstOrDefault()?.Product?.Name ?? "Unknown",
                AmountBeforeDiscount = Math.Round((o.Items.FirstOrDefault()?.Subtotal ?? 0), 2)
            });
            
            return Ok(result);
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessOrder([FromBody] ProcessOrderRequest req)
        {
            var user = await _context.Users.FindAsync(req.UserId);
            if (user == null) return NotFound("User not found");

            var station = await _context.GasStations
                .Include(s => s.Tanks)
                .ThenInclude(t => t.FuelType)
                .FirstOrDefaultAsync(s => s.Id == req.GasStationId);
            if (station == null) return NotFound("Gas station not found");

            var tank = station.Tanks.FirstOrDefault(t => t.FuelType?.Id == req.FuelTypeId);
            if (tank == null) return BadRequest("Fuel type not available at this station");

            double volume = 0;
            double amountBeforeDiscount = 0;

            if (req.IsVolumeMode)
            {
                volume = req.InputValue;
                amountBeforeDiscount = volume * tank.FuelType.Price;
            }
            else
            {
                amountBeforeDiscount = req.InputValue;
                volume = amountBeforeDiscount / tank.FuelType.Price;
            }

            if (volume <= 0) return BadRequest("Invalid quantity");
            if (volume > tank.Volume) return BadRequest("Not enough fuel in the tank");

            double totalAmount = amountBeforeDiscount;
            int bonusesToAccrue = 0;
            var card = await _context.BonusCards.FirstOrDefaultAsync(b => b.UserId == req.UserId);

            if (req.UseBonuses)
            {
                if (card != null)
                {
                    if (card.BonusBalance >= req.BonusesToSpend)
                    {
                        card.BonusBalance -= req.BonusesToSpend;
                        totalAmount -= req.BonusesToSpend; // 1 bonus = 1 PLN discount
                        if (totalAmount < 0) totalAmount = 0;
                    }
                    else
                    {
                        return BadRequest("Not enough bonuses to spend.");
                    }
                }
                else
                {
                    return BadRequest("User does not have a bonus card.");
                }
            }

            // Bonus logic: 10% of total amount spent
            bonusesToAccrue = (int)Math.Round(totalAmount * 0.10);

            if (card != null)
            {
                card.BonusBalance += bonusesToAccrue;
            }

            // Deduct fuel from tank
            tank.Volume -= volume;

            var order = new Order
            {
                Date = DateTime.UtcNow,
                UserId = req.UserId,
                GasStationId = req.GasStationId,
                PumpId = req.PumpId,
                PaymentCardNumber = req.PaymentCardNumber,
                AccruedBonuses = bonusesToAccrue,
                PaymentStatus = "Paid",
                BonusSpent = req.UseBonuses ? req.BonusesToSpend : 0
            };

            // Use Good instead of Fuel to avoid complex relationships and constructor requirements
            order.Items.Add(new CartItem 
            { 
                Quantity = volume, 
                Product = new Good { Name = tank.FuelType.Name, Price = tank.FuelType.Price } 
            });


            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new { 
                OrderId = order.Id, 
                TotalPaid = Math.Round(totalAmount, 2), 
                BonusesEarned = bonusesToAccrue,
                Volume = Math.Round(volume, 2),
                FuelName = tank.FuelType.Name,
                AmountBeforeDiscount = Math.Round(amountBeforeDiscount, 2)
            });
        }
    }

    public class ProcessOrderRequest
    {
        public int UserId { get; set; }
        public int GasStationId { get; set; }
        public int PumpId { get; set; }
        public int FuelTypeId { get; set; }
        public double InputValue { get; set; }
        public bool IsVolumeMode { get; set; }
        public bool UseBonuses { get; set; }
        public int BonusesToSpend { get; set; }
        public string? PaymentCardNumber { get; set; }
    }
}
