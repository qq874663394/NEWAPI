using Microsoft.EntityFrameworkCore;

namespace Repositories.Persistence.Providers;

public class PostgreSqlDatabaseProvider
    : IDatabaseProvider
{
    public string Name => "PostgreSql";

    public void Configure(
        DbContextOptionsBuilder options,
        string connectionString)
    {
        options.UseNpgsql(connectionString);
    }
}