using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IDatabaseProvider
{
    string ProviderName { get; }

    void Configure(
        DbContextOptionsBuilder options,
        string connectionString);
}