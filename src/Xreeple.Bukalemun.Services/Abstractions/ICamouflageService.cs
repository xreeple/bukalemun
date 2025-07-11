namespace Xreeple.Bukalemun.Services.Abstractions;
public interface ICamouflageService
{
    void Create(string store, string tableName, string primaryKey, string columnName, string value);
}
