using gsst.Model;
using gsst.Model.User;
using gsst.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetrolAPI.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetrolAPI.Services
{
    public class ApiCustomerAuthService : IApiCustomerAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ApiCustomerAuthService(AppDbContext db, IConfiguration configuration)
        {
            _context = db;
            _configuration = configuration;
        }

        public async Task<bool> IsPhoneExistsAsync(string phoneNumber)
        {
            return await _context.Users.AnyAsync(u => u.PhoneNumber != null && u.PhoneNumber.ToLower() == phoneNumber.ToLower());
        }

        public async Task<LoginResponceDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var input = loginRequestDTO.PhoneNumber ?? "";
                var cleanPhone = input.Replace(" ", "").Replace("-", "");
                var inputLower = input.ToLower();
                var cleanPhoneLower = cleanPhone.ToLower();

                var user = await _context.Users.FirstOrDefaultAsync(u => 
                    (u.PhoneNumber != null && u.PhoneNumber.ToLower() == cleanPhoneLower) ||
                    (u.Username != null && u.Username.ToLower() == inputLower) ||
                    (u.Email != null && u.Email.ToLower() == inputLower)
                );

                if (user == null || user.Password != loginRequestDTO.Password)
                {
                    return null;
                }

                var bonusCard = await _context.BonusCards.FirstOrDefaultAsync(b => b.UserId == user.Id);

                var token = GenerateJwtToken(user);

                return new LoginResponceDTO()
                {
                    User = new ClientDTO()
                    {
                        Id = user.Id,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        Name = user.FullName,
                        Role = user.Role,
                        Barcode = bonusCard?.Barcode,
                        BonusBalance = bonusCard?.BonusBalance ?? 0,
                        GasStationId = user.GasStationId
                    },
                    Token = token
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to login the user", ex);
            }
        }

        public async Task<ClientDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                if (await IsPhoneExistsAsync(registrationRequestDTO.PhoneNumber))
                {
                    throw new InvalidOperationException($"User with phone number {registrationRequestDTO.PhoneNumber} already exists");
                }
                if (await _context.Users.AnyAsync(u => u.Email != null && u.Email.ToLower() == registrationRequestDTO.Email.ToLower()))
                {
                    throw new InvalidOperationException($"User with email {registrationRequestDTO.Email} already exists");
                }

                User user = new()
                {
                    PhoneNumber = registrationRequestDTO.PhoneNumber,
                    Email = registrationRequestDTO.Email,
                    FullName = registrationRequestDTO.ClientName,
                    Username = new string(registrationRequestDTO.PhoneNumber.Where(char.IsLetterOrDigit).ToArray()), // Use sanitized phone as username
                    Password = registrationRequestDTO.Password,
                    Role = UserRoles.Client
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                // Выдача 20 бонусов за первую регистрацию
                BonusCard bonusCard = new()
                {
                    UserId = user.Id,
                    ClientName = user.FullName,
                    Barcode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                    BonusBalance = 20
                };
                await _context.BonusCards.AddAsync(bonusCard);
                await _context.SaveChangesAsync();

                return new ClientDTO()
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Name = user.FullName,
                    Role = user.Role,
                    Barcode = bonusCard.Barcode,
                    BonusBalance = bonusCard.BonusBalance,
                    GasStationId = user.GasStationId
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to register user", ex);
            }
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtSettings")["Secret"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.FullName)
            };

            if (user.GasStationId.HasValue) claims.Add(new Claim("GasStationId", user.GasStationId.Value.ToString()));

            if (!string.IsNullOrEmpty(user.Email)) claims.Add(new Claim(ClaimTypes.Email, user.Email));
            if (!string.IsNullOrEmpty(user.PhoneNumber)) claims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(90),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
