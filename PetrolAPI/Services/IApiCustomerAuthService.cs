using gsst.Model.User;
using PetrolAPI.DTO;

namespace PetrolAPI.Services
{
    public interface IApiCustomerAuthService
    {
        Task<bool> IsPhoneExistsAsync(string email);
        
        Task<ClientDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);

        Task<LoginResponceDTO?> LoginAsync(LoginRequestDTO loginRequestDTO);
    }
}
