using Application.DTO.Auth;
using System.Security.Claims;

namespace Application.Interfaces.Auth
{
    public interface ITokenService
    {
        JwtTokenResult CreateToken(Guid userCode, string userName = "");
        Guid? ValidateToken(string token);
        JwtTokenResult RefreshToken(string token);
    }
}
