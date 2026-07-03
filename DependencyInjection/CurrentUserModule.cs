using Infrastructure.Authentication.CurrentUser;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection;

public static class CurrentUserModule
{
    public static IServiceCollection AddCurrentUserModule(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}