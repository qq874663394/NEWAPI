using Application.Auth;

namespace Infrastructure.Authentication;

public class AuthenticationProviderRegistry : IAuthenticationProviderRegistry
{
    private readonly IEnumerable<IAuthenticationProvider> _providers;

    public AuthenticationProviderRegistry(IEnumerable<IAuthenticationProvider> providers)
    {
        _providers = providers;
    }

    public IAuthenticationProvider GetProvider(string key)
    {
        var provider = _providers.FirstOrDefault(x =>
            x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
            throw new Exception($"未找到认证提供者: {key}");

        return provider;
    }
}