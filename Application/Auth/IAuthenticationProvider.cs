using Application.Auth.DTO;
using Domain.Entities;

namespace Application.Auth
{
    public interface IAuthenticationProvider
    {
        string Key { get; }

        Task<SysUser?> AuthenticateAsync(AuthenticationRequest request);
    }
}