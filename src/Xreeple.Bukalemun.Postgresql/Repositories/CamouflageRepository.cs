using Dapper;
using Xreeple.Bukalemun.Data.Abstractions;
using Xreeple.Bukalemun.Data.Entites;

namespace Xreeple.Bukalemun.Postgresql.Repositories;

public class CamouflageRepository(IDbContext _dbContext) : ICamouflageRepository
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
        using var connection = _dbContext.CreateConnection();

        var sql = $"""
                SELECT * FROM "{store}"
                WHERE "TableName" = @TableName
                AND "PrimaryKey" = @PrimaryKey
                AND "ColumnName" = @ColumnName
            """;

        return connection.QueryFirstOrDefault<Camouflaged>(
            sql,
            new
            {
                TableName = tableName,
                PrimaryKey = primaryKey,
                ColumnName = columnName,
            }
        );
    }
}
