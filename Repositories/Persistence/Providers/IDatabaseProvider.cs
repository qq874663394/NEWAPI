using Microsoft.EntityFrameworkCore;

namespace Repositories.Persistence.Providers;

public interface IDatabaseProvider
{
    string Name { get; }

    void Configure(
        DbContextOptionsBuilder options,
        string connectionString);
}