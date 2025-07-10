using Dapper;
using Npgsql;
using System.Data;
using Xreeple.Bukalemun.Data.Abstractions;

namespace Xreeple.Bukalemun.Data.Postgresql;
public class PostgresqlDbContext(string connectionString, string schema) : IDbContext
{
    public IDbConnection CreateConnection() => new NpgsqlConnection(connectionString);

    public void Migration()
    {
        using var connection = CreateConnection();

        string defaultCamouflagedTableName = "Default";

        if (schema != "bukalemun")
        {
            defaultCamouflagedTableName = "bukalemun." + defaultCamouflagedTableName;
        }

        var sql = $"""
            CREATE SCHEMA IF NOT EXISTS {schema};

            SET search_path = '{schema}';

            CREATE TABLE IF NOT EXISTS "{defaultCamouflagedTableName}" (
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
