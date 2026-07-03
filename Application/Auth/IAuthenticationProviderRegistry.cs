namespace Application.Auth
{

    public interface IAuthenticationProviderRegistry
    {
        public IAuthenticationProvider GetProvider(string key);
    }
}
