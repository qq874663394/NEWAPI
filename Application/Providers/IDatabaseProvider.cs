using Microsoft.EntityFrameworkCore;

namespace Application.Providers;

public interface IDatabaseProvider
{
    string ProviderName { get; }

    void Configure(
        DbContextOptionsBuilder options,
        string connectionString);
}