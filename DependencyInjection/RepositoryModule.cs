using Domain.Interface.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjection
{
    public static class RepositoryModule
    {
        public static IServiceCollection AddRepositoryModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<WebApiDbContext>(...);

            services.AddScoped<
                IRepositoryContext,
                WebApiRepositoryContext>();

            services.AddScoped(
                typeof(IRepository<>),
                typeof(WebApiRepository<>));

            return services;
        }
    }
}
