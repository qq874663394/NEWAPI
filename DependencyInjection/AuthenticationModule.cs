using Microsoft.Extensions.DependencyInjection;
using Repositories.Authentication;
using Domain.Interface.Services.Authentication;
using Repositories.Authentication.Providers;
using Repositories.Authentication.Security;

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
