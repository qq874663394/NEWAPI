using Application.DTO.Auth;

namespace Application.Interfaces.Auth;

public interface IAuthService
{
    Task<LoginResult?> LoginAsync(AuthenticationRequest request);

    string RefreshToken(string token);
}