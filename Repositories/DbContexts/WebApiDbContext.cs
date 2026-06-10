using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Repositories.WebApiDB
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext()
        {
        }

        public WebApiDbContext(DbContextOptions<WebApiDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(WebApiDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
