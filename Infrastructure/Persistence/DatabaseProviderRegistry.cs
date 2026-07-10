using Application.Interfaces;
using System.Collections.Concurrent;

namespace Repositories.Persistence;

public class DatabaseProviderRegistry
{
    private readonly ConcurrentDictionary<string, IDatabaseProvider> _providers;

    public DatabaseProviderRegistry(
        IEnumerable<IDatabaseProvider> providers)
    {
        _providers = new ConcurrentDictionary<string, IDatabaseProvider>(
            providers.ToDictionary(
                x => x.ProviderName,
                StringComparer.OrdinalIgnoreCase));
    }

    public IDatabaseProvider GetProvider(
        string providerName)
    {
        if (_providers.TryGetValue(providerName, out var provider))
            return provider;

        throw new NotSupportedException(
            $"不支持数据库提供程序 [{providerName}]。");
    }
}