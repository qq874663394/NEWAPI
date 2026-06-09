using Microsoft.EntityFrameworkCore;

namespace Repositories.Persistence.Providers;

public class MySqlDatabaseProvider
    : IDatabaseProvider
{
    public string Name => "MySql";

    public void Configure(
        DbContextOptionsBuilder options,
        string connectionString)
    {
        options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString));
    }
}