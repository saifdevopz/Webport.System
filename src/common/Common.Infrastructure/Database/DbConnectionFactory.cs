using Common.Application.Database;
using Npgsql;
using System.Data.Common;

namespace Common.Infrastructure.Database;

internal sealed class DbConnectionFactory(CurrentConnection ct) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenPostgreSQLConnection(string? connectionString = null)
    {
        NpgsqlConnection connection = !string.IsNullOrEmpty(connectionString) ?
            new NpgsqlConnection(connectionString) :
            new NpgsqlConnection(ct.GetParentConnectionString());

        await connection.OpenAsync();

        return connection;
    }
}