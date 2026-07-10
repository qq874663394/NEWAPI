using Application.DTO.Auth;
using Application.Interfaces.Auth;
using Domain.Entities;

namespace Infrastructure.Authentication.Providers;

public class WeChatAuthenticationProvider : IAuthenticationProvider
{
    public string Key => "WeChat";

    public Task<SysUser?> AuthenticateAsync(AuthenticationRequest request)
    {
        throw new NotImplementedException();
    }
}