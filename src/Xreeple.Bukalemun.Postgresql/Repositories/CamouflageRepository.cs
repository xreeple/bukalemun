using Dapper;
using Xreeple.Bukalemun.Data.Abstractions;
using Xreeple.Bukalemun.Data.Entites;

namespace Xreeple.Bukalemun.Postgresql.Repositories;

internal class CamouflageRepository(IDbContext _dbContext) : ICamouflageRepository
{
    public bool Upsert(Camouflaged camouflaged)
    {
        using var connection = _dbContext.CreateConnection();

        var sql = $"""
                INSERT INTO "{camouflaged.Store}" (
                    "TableName", 
                    "PrimaryKey", 
                    "ColumnName", 
                    "Encrypted", 
                    "Hashed", 
                    "CreatedAt", 
                    "UpdatedAt"
                )
                VALUES (
                    @TableName, 
                    @PrimaryKey, 
                    @ColumnName, 
                    @Encrypted, 
                    @Hashed, 
                    @CreatedAt, 
                    @UpdatedAt
                )
                ON CONFLICT ("TableName", "PrimaryKey", "ColumnName")
                DO UPDATE SET
                    "Encrypted" = EXCLUDED."Encrypted",
                    "Hashed" = EXCLUDED."Hashed",
                    "UpdatedAt" = EXCLUDED."UpdatedAt"
            """;

        return connection.Execute(sql, camouflaged) == 1;
    }

    public Camouflaged? Get(string store, string tableName, string primaryKey, string columnName)
    {
        return Get(store, tableName, [primaryKey], [columnName]).FirstOrDefault();
    }

    public IEnumerable<Camouflaged> Get(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    )
    {
        return Get(store, tableName, primaryKeys, [columnName]);
    }

    public IEnumerable<Camouflaged> Get(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    )
    {
        return Get(store, tableName, [primaryKey], columnNames);
    }

    public IEnumerable<Camouflaged> Get(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    )
    {
        using var connection = _dbContext.CreateConnection();

        var sql = $"""
                SELECT 
                    "TableName", 
                    "PrimaryKey", 
                    "ColumnName", 
                    "Encrypted", 
                    "Hashed"
                FROM "{store}"
                WHERE "TableName" = @TableName
                AND "PrimaryKey" = ANY(@PrimaryKeys)
                AND "ColumnName" = ANY(@ColumnNames)
            """;

        return connection.Query<Camouflaged>(
            sql,
            new
            {
                TableName = tableName,
                PrimaryKeys = primaryKeys,
                ColumnNames = columnNames,
            }
        );
    }
}
