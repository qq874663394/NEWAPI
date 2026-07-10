using Application.DTO.Auth;
using Domain.Entities;

namespace Application.Interfaces.Auth
{
    public interface IAuthenticationProvider
    {
        string Key { get; }

        Task<SysUser?> AuthenticateAsync(AuthenticationRequest request);
    }
}