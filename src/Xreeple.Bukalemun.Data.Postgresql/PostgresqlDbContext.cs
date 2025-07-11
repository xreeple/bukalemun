using Dapper;
using Npgsql;
using System.Data;
using Xreeple.Bukalemun.Data.Abstractions;

namespace Xreeple.Bukalemun.Data.Postgresql;

public class PostgresqlDbContext(string _connectionString, string _schema) : IDbContext
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
                        "TableName" TEXT NOT NULL,
                        "PrimaryKey" TEXT NOT NULL,
                        "ColumnName" TEXT NOT NULL,
                        "Encrypted" bytea,
                        "Hashed" TEXT,
                        "CreatedAt" TIMESTAMP NOT NULL,
                        "UpdatedAt" TIMESTAMP NOT NULL,
                        PRIMARY KEY ("TableName", "PrimaryKey", "ColumnName")
                    );
                """;

            connection.Execute(sql);
        }
    }
}
