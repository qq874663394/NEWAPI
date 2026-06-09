using Domain.Entities;
using Domain.Interface.IServices.Authentication;
using Domain.Interface.Services.Authentication;
using Domain.Model.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
