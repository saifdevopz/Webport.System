using System.Data.Common;

namespace Common.Application.Database;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenPostgreSQLConnection(string? connectionString = null);
}
