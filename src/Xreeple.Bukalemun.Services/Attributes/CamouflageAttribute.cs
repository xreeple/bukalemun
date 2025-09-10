namespace Xreeple.Bukalemun.Services.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CamouflageAttribute : Attribute
{
    public string Store;
    public string TableName;

    public CamouflageAttribute(string store)
    {
        Store = store;
        TableName = "default";
    }

    public CamouflageAttribute(string store, string tableName)
    {
        Store = store;
        TableName = tableName;
    }
}
