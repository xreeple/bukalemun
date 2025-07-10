using System.Data;

namespace Xreeple.Bukalemun.Data.Abstractions;
public interface IDbContext
{
    IDbConnection CreateConnection();
    void Migration();
}