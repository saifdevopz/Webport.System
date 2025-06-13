using Common.Application.Database;
using Dapper;
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

    public async Task<List<T>> QueryAsync<T>(string sql, object parameters = null!, bool systemDb = false)
    {
        using DbConnection connection = await OpenPostgreSQLConnection();
        //await connection.OpenAsync();

        IEnumerable<T> result = await connection.QueryAsync<T>(sql, parameters);
        return [.. result];
    }
}