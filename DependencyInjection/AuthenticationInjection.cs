using Application.Auth;
using Infrastructure.Authentication;
using Infrastructure.Authentication.Providers;
using Infrastructure.Authentication.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection;

public static class AuthenticationInjection
{
    public static IServiceCollection AddAuthenticationModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        // =========================
        // Jwt Options 注入
        // =========================
        services.Configure<JwtOptions>(
            configuration.GetSection("Jwt"));

        // =========================
        // Auth Core（接口 → 实现）
        // =========================
        services.AddScoped<IAuthService, AuthService>();

        // 把具体类注册改成接口注册，并添加 ITokenService 映射
        services.AddScoped<ITokenService, JwtTokenService>();        // 关键：添加接口映射
        services.AddScoped<IAuthenticationProviderRegistry, AuthenticationProviderRegistry>(); // 关键：添加接口映射

        // 如果其他地方需要直接注入具体类，可以保留（但通常不需要）
        // services.AddScoped<JwtTokenService>();
        // services.AddScoped<AuthenticationProviderRegistry>();

        // =========================
        // Providers（接口 → 实现）
        // =========================
        services.AddScoped<IAuthenticationProvider, PasswordAuthenticationProvider>();
        services.AddScoped<IAuthenticationProvider, ApoAuthenticationProvider>();
        services.AddScoped<IAuthenticationProvider, LdapAuthenticationProvider>();
        services.AddScoped<IAuthenticationProvider, WindowsAuthenticationProvider>();
        services.AddScoped<IAuthenticationProvider, OAuthAuthenticationProvider>();
        services.AddScoped<IAuthenticationProvider, WeChatAuthenticationProvider>();

        return services;
    }
}