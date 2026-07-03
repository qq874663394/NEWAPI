namespace Domain.Interface.UnitOfWorks
{
    //public interface IUnitOfWork<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    //{
    //    int Commit();
    //    Task<int> CommitAsync();
    //}

    public interface IUnitOfWork
    {
        int Commit();
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
