using Domain.Interface.Repositories;
using Domain.Interface.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.DbContexts;
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
            this IServiceCollection services)
        {
            // 通用仓储
            services.AddScoped(
                typeof(IRepository<>),
                typeof(Repository<>));

            // 工作单元
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            return services;
        }
    }
}
