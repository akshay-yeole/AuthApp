using AuthApi.Models;

namespace AuthApi.Contracts
{
    public interface ITokenService
    {
        Task<AuthResponse> GenerateTokenAsync(User user);
    }
}
