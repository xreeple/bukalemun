using Dapper;
using Xreeple.Bukalemun.Data.Abstractions;
using Xreeple.Bukalemun.Data.Entites;

namespace Xreeple.Bukalemun.Postgresql.Repositories;

internal class CamouflageRepository(IDbContext _dbContext) : ICamouflageRepository
{
    public async Task<bool> UpsertAsync(Camouflaged camouflaged)
    {
        using var connection = _dbContext.CreateConnection();

        var sql = $"""
                INSERT INTO "{camouflaged.Store}" (
                    "TableName", 
                    "PrimaryKey", 
                    "ColumnName", 
                    "Encrypted", 
                    "CreatedAt", 
                    "UpdatedAt"
                )
                VALUES (
                    @TableName, 
                    @PrimaryKey, 
                    @ColumnName, 
                    @Encrypted, 
                    @CreatedAt, 
                    @UpdatedAt
                )
                ON CONFLICT ("TableName", "PrimaryKey", "ColumnName")
                DO UPDATE SET
                    "Encrypted" = EXCLUDED."Encrypted",
                    "UpdatedAt" = EXCLUDED."UpdatedAt"
            """;

        return await connection.ExecuteAsync(sql, camouflaged) == 1;
    }

    public async Task<Camouflaged?> GetAsync(
        string store,
        string tableName,
        string primaryKey,
        string columnName
    )
    {
        return (await GetAsync(store, tableName, [primaryKey], [columnName])).FirstOrDefault();
    }

    public async Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    )
    {
        return await GetAsync(store, tableName, primaryKeys, [columnName]);
    }

    public async Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    )
    {
        return await GetAsync(store, tableName, [primaryKey], columnNames);
    }

    public async Task<IEnumerable<Camouflaged>> GetAsync(
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
                    "Encrypted"
                FROM "{store}"
                WHERE "TableName" = @TableName
                AND "PrimaryKey" = ANY(@PrimaryKeys)
                AND "ColumnName" = ANY(@ColumnNames)
            """;

        return await connection.QueryAsync<Camouflaged>(
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
