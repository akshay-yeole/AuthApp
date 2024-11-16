using AuthApi.Contracts;
using AuthApi.Models;

namespace AuthApi.Repositories
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> RegisterAsync(string username, string password)
        {
            var user = new User { Username = username, PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) };
            await _userRepository.AddUserAsync(user);
            return await _tokenService.GenerateTokenAsync(user);
        }

        public async Task<AuthResponse> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password");
            }
            return await _tokenService.GenerateTokenAsync(user);
        }
    }
}
