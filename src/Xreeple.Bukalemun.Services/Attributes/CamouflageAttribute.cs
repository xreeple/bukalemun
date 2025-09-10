namespace Xreeple.Bukalemun.Services.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CamouflageAttribute : Attribute
{
    public string Store;
    public string Table;

    public CamouflageAttribute(string store)
    {
        Store = store;
        Table = "default";
    }

    public CamouflageAttribute(string store, string table)
    {
        Store = store;
        Table = table;
    }
}
