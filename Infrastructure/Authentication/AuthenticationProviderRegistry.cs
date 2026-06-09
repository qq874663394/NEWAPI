using Domain.Interface.IServices.Authentication;

namespace Infrastructure.Authentication
{
    public class AuthenticationProviderRegistry
    {
        private readonly Dictionary<string, IAuthenticationProvider> _providers;

        public AuthenticationProviderRegistry(
            IEnumerable<IAuthenticationProvider> providers)
        {
            _providers = providers.ToDictionary(x => x.Key);
        }

        public IAuthenticationProvider Get(string key)
        {
            if (!_providers.TryGetValue(key, out var provider))
            {
                throw new Exception($"未找到认证插件: {key}");
            }

            return provider;
        }
    }

}
