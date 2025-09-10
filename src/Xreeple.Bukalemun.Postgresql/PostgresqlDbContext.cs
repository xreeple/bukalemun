using System.Data;
using Dapper;
using Npgsql;
using Xreeple.Bukalemun.Data.Abstractions;

namespace Xreeple.Bukalemun.Postgresql;

internal class PostgresqlDbContext(string _connectionString, string _schema) : IDbContext
{
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);

        connection.Open();
        connection.Execute($"SET search_path = '{_schema}'");

        return connection;
    }

    public void Migration(HashSet<string> stores)
    {
        if (stores.Count == 0)
            return;

        using var connection = CreateConnection();

        foreach (var store in stores)
        {
            var sql = $"""
                    CREATE SCHEMA IF NOT EXISTS "{_schema}";

                    SET search_path = '{_schema}';

                    CREATE TABLE IF NOT EXISTS "{store}" (
                        "Table" TEXT NOT NULL,
                        "Key" TEXT NOT NULL,
                        "Column" TEXT NOT NULL,
                        "Encrypted" bytea,
                        "CreatedAt" TIMESTAMP NOT NULL,
                        "UpdatedAt" TIMESTAMP NOT NULL,
                        PRIMARY KEY ("Table", "Key", "Column")
                    );
                """;

            connection.Execute(sql);
        }
    }
}
