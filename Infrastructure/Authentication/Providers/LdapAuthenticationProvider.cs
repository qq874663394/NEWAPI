using Domain.Entities;
using Domain.Interface.Services.Authentication;
using Domain.Model.Authentication;

namespace Infrastructure.Authentication.Providers
{
    public class LdapAuthenticationProvider
            : IAuthenticationProvider
    {
        public string Key => "Ldap";

        public Task<T_User?> AuthenticateAsync(
            AuthenticationRequest request)
        {
            throw new NotImplementedException(
                "LDAP认证暂未实现");
        }
    }
}
