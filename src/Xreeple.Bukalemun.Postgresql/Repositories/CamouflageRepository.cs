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
                    "Column", 
                    "Encrypted", 
                    "CreatedAt", 
                    "UpdatedAt"
                )
                VALUES (
                    @Table, 
                    @PrimaryKey, 
                    @Column, 
                    @Encrypted, 
                    @CreatedAt, 
                    @UpdatedAt
                )
                ON CONFLICT ("Table", "PrimaryKey", "Column")
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
        string column
    )
    {
        return (await GetAsync(store, table, [primaryKey], [column])).FirstOrDefault();
    }

    public async Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string[] primaryKeys,
        string column
    )
    {
        return await GetAsync(store, table, primaryKeys, [column]);
    }

    public async Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string primaryKey,
        string[] columns
    )
    {
        return await GetAsync(store, table, [primaryKey], columns);
    }

    public async Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string[] primaryKeys,
        string[] columns
    )
    {
        using var connection = _dbContext.CreateConnection();

        var sql = $"""
                SELECT 
                    "Table", 
                    "PrimaryKey", 
                    "Column", 
                    "Encrypted"
                FROM "{store}"
                WHERE "Table" = @Table
                AND "PrimaryKey" = ANY(@PrimaryKeys)
                AND "Column" = ANY(@Columns)
            """;

        return await connection.QueryAsync<Camouflaged>(
            sql,
            new
            {
                Table = table,
                PrimaryKeys = primaryKeys,
                Columns = columns,
            }
        );
    }
}
