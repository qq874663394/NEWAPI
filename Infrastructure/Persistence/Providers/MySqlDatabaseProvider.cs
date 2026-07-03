using Application.Providers;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Persistence.Providers;

public class MySqlDatabaseProvider
    : IDatabaseProvider
{
    public string ProviderName => "MySql";

    public void Configure(
        DbContextOptionsBuilder options,
        string connectionString)
    {
        options.UseMySQL(connectionString);
    }
}