using System.Data;

namespace Xreeple.Bukalemun.Data.Abstractions;

internal interface IDbContext
{
    IDbConnection CreateConnection();
    void Migration(HashSet<string> stores);
}
