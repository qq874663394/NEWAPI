using Domain.Interface.IServices.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Authentication.Providers;
using Infrastructure.Authentication.Security;
using Infrastructure.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace DependencyInjection
{
    public static class AuthenticationModule
    {
        public static IServiceCollection AddAuthenticationModule(
       this IServiceCollection services)
        {
            services.AddScoped<AuthService, AuthService>();
            services.AddScoped<ITokenService, JwtTokenService>();

            // 自动注册所有插件
            services.AddScoped<AuthenticationProviderRegistry>();

            services.AddScoped<IAuthenticationProvider, PasswordAuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, ApoAuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, WindowsAuthenticationProvider>();
            services.AddScoped<IAuthenticationProvider, LdapAuthenticationProvider>();

            return services;
        }
    }
}
