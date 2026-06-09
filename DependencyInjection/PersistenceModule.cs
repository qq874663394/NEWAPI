using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Persistence;
using Repositories.Persistence.Providers;

namespace DependencyInjection;

public static class PersistenceModule
{
    public static IServiceCollection AddPersistenceModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IDatabaseProvider,
            SqlServerDatabaseProvider>();

        services.AddScoped<IDatabaseProvider,
            MySqlDatabaseProvider>();

        services.AddScoped<IDatabaseProvider,
            PostgreSqlDatabaseProvider>();

        services.AddScoped<DatabaseProviderRegistry>();

        services.AddDbContext<WebApiDbContext>(
            (sp, options) =>
            {
                var registry =
                    sp.GetRequiredService<DatabaseProviderRegistry>();

                var providerName =
                    configuration["Database:Provider"];

                var provider =
                    registry.GetProvider(providerName);

                var connectionString =
                    configuration.GetConnectionString(providerName);

                provider.Configure(
                    options,
                    connectionString);
            });

        return services;
    }
}