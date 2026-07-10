using Repositories.DbContexts;

namespace Domain.Interfaces.UnitOfWorks
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly WebApiDbContext _dbContext;

        public EfUnitOfWork(
            WebApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }
        public Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(
                cancellationToken);
        }
    }
}
