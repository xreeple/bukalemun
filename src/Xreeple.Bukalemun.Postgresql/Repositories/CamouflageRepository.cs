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
                    "Table", 
                    "PrimaryKey", 
                    "ColumnName", 
                    "Encrypted", 
                    "CreatedAt", 
                    "UpdatedAt"
                )
                VALUES (
                    @Table, 
                    @PrimaryKey, 
                    @ColumnName, 
                    @Encrypted, 
                    @CreatedAt, 
                    @UpdatedAt
                )
                ON CONFLICT ("Table", "PrimaryKey", "ColumnName")
                DO UPDATE SET
                    "Encrypted" = EXCLUDED."Encrypted",
                    "UpdatedAt" = EXCLUDED."UpdatedAt"
            """;

        return await connection.ExecuteAsync(sql, camouflaged) == 1;
    }

    public async Task<Camouflaged?> GetAsync(
        string store,
        string table,
        string primaryKey,
        string columnName
    )
    {
        return (await GetAsync(store, table, [primaryKey], [columnName])).FirstOrDefault();
    }

    public async Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string[] primaryKeys,
        string columnName
    )
    {
        return await GetAsync(store, table, primaryKeys, [columnName]);
    }

    public async Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string primaryKey,
        string[] columnNames
    )
    {
        return await GetAsync(store, table, [primaryKey], columnNames);
    }

    public async Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string[] primaryKeys,
        string[] columnNames
    )
    {
        using var connection = _dbContext.CreateConnection();

        var sql = $"""
                SELECT 
                    "Table", 
                    "PrimaryKey", 
                    "ColumnName", 
                    "Encrypted"
                FROM "{store}"
                WHERE "Table" = @Table
                AND "PrimaryKey" = ANY(@PrimaryKeys)
                AND "ColumnName" = ANY(@ColumnNames)
            """;

        return await connection.QueryAsync<Camouflaged>(
            sql,
            new
            {
                Table = table,
                PrimaryKeys = primaryKeys,
                ColumnNames = columnNames,
            }
        );
    }
}
