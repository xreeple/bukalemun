using Dapper;
using Microsoft.VisualBasic;
using Xreeple.Bukalemun.Data.Abstractions;
using Xreeple.Bukalemun.Data.Entites;

namespace Xreeple.Bukalemun.Postgresql.Repositories;

internal class CamouflageRepository(IDbContext _dbContext) : ICamouflageRepository
{
    public async Task<bool> InsertAsync(Camouflaged camouflaged)
    {
        using var connection = _dbContext.CreateConnection();

        var sql = $"""
                INSERT INTO "{camouflaged.Store}" (
                    "Table", 
                    "Key", 
                    "Column", 
                    "Encrypted", 
                    "CreatedAt", 
                    "UpdatedAt"
                )
                VALUES (
                    @Table, 
                    @Key, 
                    @Column, 
                    @Encrypted, 
                    @CreatedAt, 
                    @UpdatedAt
                )
                ON CONFLICT ("Table", "Key", "Column") 
                DO NOTHING
            """;

        return await connection.ExecuteAsync(sql, camouflaged) == 1;
    }

    public async Task<bool> UpsertAsync(Camouflaged camouflaged)
    {
        using var connection = _dbContext.CreateConnection();

        var sql = $"""
                INSERT INTO "{camouflaged.Store}" (
                    "Table", 
                    "Key", 
                    "Column", 
                    "Encrypted", 
                    "CreatedAt", 
                    "UpdatedAt"
                )
                VALUES (
                    @Table, 
                    @Key, 
                    @Column, 
                    @Encrypted, 
                    @CreatedAt, 
                    @UpdatedAt
                )
                ON CONFLICT ("Table", "Key", "Column")
                DO UPDATE SET
                    "Encrypted" = EXCLUDED."Encrypted",
                    "UpdatedAt" = EXCLUDED."UpdatedAt"
            """;

        return await connection.ExecuteAsync(sql, camouflaged) == 1;
    }

    public async Task<Camouflaged?> GetAsync(string store, string table, string key, string column)
    {
        return (await GetAsync(store, table, [key], [column])).FirstOrDefault();
    }

    public async Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string[] keys,
        string column
    )
    {
        return await GetAsync(store, table, keys, [column]);
    }

    public async Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string key,
        string[] columns
    )
    {
        return await GetAsync(store, table, [key], columns);
    }

    public async Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string[] keys,
        string[] columns
    )
    {
        using var connection = _dbContext.CreateConnection();

        var sql = $"""
                SELECT 
                    "Table", 
                    "Key", 
                    "Column", 
                    "Encrypted"
                FROM "{store}"
                WHERE "Table" = @Table
                AND "Key" = ANY(@Keys)
                AND "Column" = ANY(@Columns)
            """;

        return await connection.QueryAsync<Camouflaged>(
            sql,
            new
            {
                Table = table,
                Keys = keys,
                Columns = columns,
            }
        );
    }
}
