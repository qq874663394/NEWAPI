using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories.DbContexts;
using Repositories.Persistence;
using Repositories.Persistence.Providers;   // 引入具体实现

namespace DependencyInjection;

public static class PersistenceModule
{
    public static IServiceCollection AddPersistenceModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // =========================
        // 注册所有数据库提供者（必须）
        // 因为它们是无状态的，可以注册为 Singleton
        // =========================
        services.AddSingleton<IDatabaseProvider, SqlServerDatabaseProvider>();
        services.AddSingleton<IDatabaseProvider, MySqlDatabaseProvider>();
        services.AddSingleton<IDatabaseProvider, PostgreSqlDatabaseProvider>();

        // 注册提供者注册表（也设为 Singleton，与提供者生命周期一致）
        services.AddSingleton<DatabaseProviderRegistry>();

        // DbContext（通常 Scoped，不要改 Singleton）
        services.AddDbContext<WebApiDbContext>((sp, options) =>
        {
            var registry =
                sp.GetRequiredService<DatabaseProviderRegistry>();

            var providerName =
                configuration["Database:Provider"]
                ?? throw new InvalidOperationException("未配置 Database:Provider。");

            var connectionString =
                configuration.GetConnectionString(providerName)
                ?? throw new InvalidOperationException(
                    $"未找到连接字符串：{providerName}");

            registry
                .GetProvider(providerName)
                .Configure(options, connectionString);
        });

        return services;
    }
}