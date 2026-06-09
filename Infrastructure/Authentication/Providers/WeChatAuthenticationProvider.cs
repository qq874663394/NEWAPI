using Domain.Entities;
using Domain.Interface.IServices.Authentication;
using Domain.Model.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication.Providers
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
