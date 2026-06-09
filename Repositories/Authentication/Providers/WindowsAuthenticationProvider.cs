using Domain.Entities;
using Domain.Interface.Services.Authentication;
using Domain.Model.Authentication;
namespace Repositories.Authentication.Providers
{
    public class WindowsAuthenticationProvider
        : IAuthenticationProvider
    {
        public string Key => "Windows";

        public Task<T_User?> AuthenticateAsync(
            AuthenticationRequest request)
        {
            throw new NotImplementedException(
                "Windows认证暂未实现");
        }
    }
}
