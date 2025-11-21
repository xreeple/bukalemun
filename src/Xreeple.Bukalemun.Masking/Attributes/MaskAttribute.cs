namespace Xreeple.Bukalemun.Masking.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class MaskAttribute : Attribute
{
    public char MaskChar = '*';
    public int CompactMask = 0;
    public bool RemoveMasked = false;
    public string? PreserveChars;
    public bool PreserveWhitespace = false;
    public int RevealFirst = 0;
    public int RevealLast = 0;
    public int RevealRangeStart = -1;
    public int RevealRangeLength = -1;
    public string? RevealRegex;
    public int RevealInitialsPerWord = 0;
    public Func<char, int, bool>? RevealIf;
}
