namespace Xreeple.Bukalemun.Services.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CamouflageableAttribute : Attribute
{
    public string Column;

    public CamouflageableAttribute()
    {
        Column = "default";
    }

    public CamouflageableAttribute(string column)
    {
        Column = column;
    }
}
