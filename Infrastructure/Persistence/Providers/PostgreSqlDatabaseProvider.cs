using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Persistence.Providers;

public class PostgreSqlDatabaseProvider
    : IDatabaseProvider
{
    public string ProviderName => "PostgreSql";

    public void Configure(
        DbContextOptionsBuilder options,
        string connectionString)
    {
        options.UseNpgsql(connectionString);
    }
}