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
