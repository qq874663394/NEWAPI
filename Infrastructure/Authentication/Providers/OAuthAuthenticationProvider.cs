using Application.Auth;
using Application.Auth.DTO;
using Domain.Entities;
namespace Infrastructure.Authentication.Providers;

public class OAuthAuthenticationProvider : IAuthenticationProvider
{
    public string Key => "OAuth";

    public Task<SysUser?> AuthenticateAsync(AuthenticationRequest request)
    {
        throw new NotImplementedException();
    }
}