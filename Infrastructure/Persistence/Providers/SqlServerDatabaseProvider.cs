using Application.Providers;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Persistence.Providers;

public class SqlServerDatabaseProvider
    : IDatabaseProvider
{
    public string ProviderName => "SqlServer";

    public void Configure(
        DbContextOptionsBuilder options,
        string connectionString)
    {
        options.UseSqlServer(connectionString);
    }
}