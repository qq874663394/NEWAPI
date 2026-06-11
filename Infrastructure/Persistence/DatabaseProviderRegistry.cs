using Repositories.Persistence.Providers;

namespace Repositories.Persistence;

public class DatabaseProviderRegistry
{
    private readonly IEnumerable<IDatabaseProvider> _providers;

    public DatabaseProviderRegistry(
        IEnumerable<IDatabaseProvider> providers)
    {
        _providers = providers;
    }

    public IDatabaseProvider GetProvider(
        string providerName)
    {
        var provider = _providers.FirstOrDefault(x =>
            x.Name.Equals(
                providerName,
                StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            throw new Exception(
                $"数据库提供程序[{providerName}]不存在");
        }

        return provider;
    }
}