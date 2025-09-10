namespace Xreeple.Bukalemun.Services.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PrimaryKeyAttribute : Attribute
{
    public int Order = 0;
}
