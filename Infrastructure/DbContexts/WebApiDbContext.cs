using Microsoft.EntityFrameworkCore;

namespace Repositories.DbContexts
{
    public partial class WebApiDbContext : DbContext
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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WebApiDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
