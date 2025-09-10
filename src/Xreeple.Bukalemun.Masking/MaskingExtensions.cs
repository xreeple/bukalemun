using System.Reflection;
using Xreeple.Bukalemun.Masking.Attributes;

namespace Xreeple.Bukalemun.Masking;

public static class MaskingExtensions
{
    public static T ApplyMasking<T>(this T obj)
    {
        var type = typeof(T);

        foreach (var prop in type.GetProperties())
        {
            if (prop.PropertyType != typeof(string))
                continue;

            var attr = prop.GetCustomAttribute<MaskAttribute>();

            if (attr == null)
                continue;

            if (prop.GetValue(obj) is not string value)
                continue;

            var builder = Mask.Build(value);

            if (attr.RevealFirst > 0)
                builder.RevealFirst(attr.RevealFirst);

            if (attr.RevealLast > 0)
                builder.RevealLast(attr.RevealLast);

            if (attr.CompactMask > 0)
                builder.CompactMask(attr.CompactMask);

            if (attr.RevealInitialsPerWord)
                builder.RevealInitialsPerWord();

            if (attr.RemoveMasked)
                builder.RemoveMasked();

            if (attr.PreserveChars is not null)
                builder.PreserveChars(attr.PreserveChars!);

            if (attr.PreserveWhitespace)
                builder.PreserveWhitespace();

            if (attr.RevealRangeStart >= 0 && attr.RevealRangeLength > 0)
                builder.RevealRange(attr.RevealRangeStart, attr.RevealRangeLength);

            if (attr.RevealRegex is not null)
                builder.RevealRegex(attr.RevealRegex!);

            if (attr.RevealIf is not null)
                builder.RevealIf(attr.RevealIf!);

            builder.MaskChar(attr.MaskChar);

            var masked = builder.ToString();
            prop.SetValue(obj, masked);
        }

        return obj;
    }
}
