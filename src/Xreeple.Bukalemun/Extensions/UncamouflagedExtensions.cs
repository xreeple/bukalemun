using System.Reflection;
using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun.Extensions;

/// <summary>
/// Extension methods for mapping <see cref="Uncamouflaged"/> objects to strongly typed instances.
/// </summary>
public static class UncamouflagedExtensions
{
    private static T MapGroupToObject<T>(IGrouping<object, Uncamouflaged> group, T obj)
    {
        var type = typeof(T);

        // Key ya da Id property set et
        var keyProp = type.GetProperty("Id") ?? type.GetProperty("Key");
        keyProp?.SetValue(obj, group.Key);

        // Diğer propertyleri doldur
        foreach (var item in group)
        {
            var prop = type.GetProperty(
                item.Name,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
            );

            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(obj, Convert.ChangeType(item.Value, prop.PropertyType));
            }
        }

        return obj;
    }

    /// <summary>
    /// Maps an enumerable of <see cref="Uncamouflaged"/> grouped by their keys to an enumerable of strongly typed objects.
    /// </summary>
    /// <typeparam name="T">The target type to map to. Must have a parameterless constructor.</typeparam>
    /// <param name="uncamouflaged">The enumerable of <see cref="Uncamouflaged"/> instances.</param>
    /// <returns>An enumerable of mapped <typeparamref name="T"/> instances.</returns>
    public static IEnumerable<T> MapTo<T>(this IEnumerable<Uncamouflaged> uncamouflaged)
        where T : new()
    {
        return uncamouflaged.GroupBy(u => u.Key).Select(group => MapGroupToObject(group, new T()));
    }

    public static IEnumerable<T> MapTo<T>(this IEnumerable<Uncamouflaged> uncamouflaged, T obj)
        where T : new()
    {
        return uncamouflaged.GroupBy(u => u.Key).Select(group => MapGroupToObject(group, obj));
    }

    /// <summary>
    /// Maps a single <see cref="Uncamouflaged"/> instance to a strongly typed object.
    /// </summary>
    /// <typeparam name="T">The target type to map to. Must have a parameterless constructor.</typeparam>
    /// <param name="uncamouflaged">The <see cref="Uncamouflaged"/> instance to map.</param>
    /// <returns>A mapped instance of <typeparamref name="T"/>.</returns>
    public static T MapTo<T>(this Uncamouflaged uncamouflaged)
        where T : new()
    {
        var obj = new T();
        var type = typeof(T);

        var keyProp = type.GetProperty("Id") ?? type.GetProperty("Key");
        keyProp?.SetValue(obj, uncamouflaged.Key);

        var prop = type.GetProperty(
            uncamouflaged.Name,
            System.Reflection.BindingFlags.IgnoreCase
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance
        );
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(obj, Convert.ChangeType(uncamouflaged.Value, prop.PropertyType));
        }

        return obj;
    }
}
