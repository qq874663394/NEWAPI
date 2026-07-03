using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationModules(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddCurrentUserModule();
        services.AddAuthenticationModule(configuration);
        services.AddRepositoryModule();
        services.AddPersistenceModule(configuration); // 运行时在Program传

        return services;
    }
}