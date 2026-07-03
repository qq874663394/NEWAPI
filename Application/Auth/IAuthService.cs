using Application.Auth.DTO;

namespace Application.Auth;

public interface IAuthService
{
    Task<LoginResult?> LoginAsync(AuthenticationRequest request);

    string RefreshToken(string token);
}