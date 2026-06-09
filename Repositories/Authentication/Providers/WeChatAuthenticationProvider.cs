using Domain.Entities;
using Domain.Interface.Services.Authentication;
using Domain.Model.Authentication;

namespace Repositories.Authentication.Providers
{
    public class WeChatAuthenticationProvider : IAuthenticationProvider
    {
        public string Key => "WeChat";

        public Task<T_User?> AuthenticateAsync(AuthenticationRequest request)
        {
            return Task.FromResult<T_User?>(null);
        }
    }
}
