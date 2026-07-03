using System.Security.Claims;

namespace Application.Auth
{
    public interface ITokenService
    {
        JwtTokenResult CreateToken(Guid userCode, string userName = "");
        Guid? ValidateToken(string token);
        JwtTokenResult RefreshToken(string token);
    }
}
