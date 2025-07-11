using System.Data;
using Dapper;
using Npgsql;
using Xreeple.Bukalemun.Data.Abstractions;

namespace Xreeple.Bukalemun.Data.Postgresql;

public class PostgresqlDbContext(string connectionString, string schema) : IDbContext
{
    public IDbConnection CreateConnection() => new NpgsqlConnection(connectionString);

    public void Migration(HashSet<string> stores)
    {
        if (stores.Count == 0)
        {
            stores.Add("Default");
        }

        using var connection = CreateConnection();

        foreach (var store in stores)
        {
            var sql = $"""
                    CREATE SCHEMA IF NOT EXISTS "{schema}";

                    SET search_path = '{schema}';

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
