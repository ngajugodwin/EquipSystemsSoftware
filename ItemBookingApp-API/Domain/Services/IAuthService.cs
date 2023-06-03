using ItemBookingApp_API.Domain.Services.Communication;

namespace ItemBookingApp_API.Domain.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(string email, string password);

        Task<AuthResponse> RefreshTokenAsync(string userRefreshToken);
    }
}
