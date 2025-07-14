using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun.Extensions;

public static class UncamouflagedExtensions
{
    public static IEnumerable<T> MapTo<T>(this IEnumerable<Uncamouflaged> uncamouflaged)
        where T : new()
    {
        var grouped = uncamouflaged.GroupBy(u => u.Key);

        foreach (var group in grouped)
        {
            var obj = new T();
            var type = typeof(T);

            var keyProp = type.GetProperty("Id") ?? type.GetProperty("Key");
            keyProp?.SetValue(obj, group.Key);

            foreach (var item in group)
            {
                var prop = type.GetProperty(
                    item.Name,
                    System.Reflection.BindingFlags.IgnoreCase
                        | System.Reflection.BindingFlags.Public
                        | System.Reflection.BindingFlags.Instance
                );
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(obj, Convert.ChangeType(item.Value, prop.PropertyType));
                }
            }

            yield return obj;
        }
    }
}
