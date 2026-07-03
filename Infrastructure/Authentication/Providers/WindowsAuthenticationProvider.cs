using Application.Auth;
using Application.Auth.DTO;
using Domain.Entities;

namespace Infrastructure.Authentication.Providers;

public class WindowsAuthenticationProvider : IAuthenticationProvider
{
    public string Key => "Windows";

    public Task<SysUser?> AuthenticateAsync(AuthenticationRequest request)
    {
        throw new NotImplementedException();
    }
}