namespace Application.Interfaces.Auth
{

    public interface IAuthenticationProviderRegistry
    {
        public IAuthenticationProvider GetProvider(string key);
    }
}
