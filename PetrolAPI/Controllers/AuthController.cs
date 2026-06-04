using gsst.Model.User;
using Microsoft.AspNetCore.Mvc;
using PetrolAPI.DTO;
using PetrolAPI.Services;

namespace PetrolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IApiCustomerAuthService _authService;

        public AuthController(IApiCustomerAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO dto)
        {
            try
            {
                var client = await _authService.RegisterAsync(dto);
                if (client == null) return BadRequest(new { message = "Registration failed." });
                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            var response = await _authService.LoginAsync(dto);
            if (response == null || string.IsNullOrEmpty(response.Token))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(response);
        }
    }
}
