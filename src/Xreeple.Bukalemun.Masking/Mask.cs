using System.Text;
using System.Text.RegularExpressions;

namespace Xreeple.Bukalemun.Masking;

public sealed class Mask
{
    private readonly string _input;
    private readonly bool[] _reveal;
    private readonly HashSet<char> _preserve;
    private char _maskChar = '*';
    private int? _compactStarCount = null;

    private Mask(string input)
    {
        _input = input ?? string.Empty;
        _reveal = new bool[_input.Length];
        _preserve = [];
    }

    public static Mask Build(string input) => new(input);

    public Mask MaskChar(char c)
    {
        _maskChar = c;
        return this;
    }

    public Mask CompactMask(int starCount)
    {
        if (starCount > 0)
            _compactStarCount = starCount;

        return this;
    }

    public Mask PreserveChars(string chars)
    {
        if (!string.IsNullOrEmpty(chars))
            foreach (var ch in chars)
                _preserve.Add(ch);

        return this;
    }

    public Mask PreserveWhitespace()
    {
        _preserve.Add(' ');
        _preserve.Add('\t');

        return this;
    }

    public Mask RevealFirst(int n)
    {
        for (int i = 0; i < Math.Min(n, _reveal.Length); i++)
            _reveal[i] = true;

        return this;
    }

    public Mask RevealLast(int n)
    {
        int len = _reveal.Length;

        for (int i = Math.Max(0, len - n); i < len; i++)
            _reveal[i] = true;

        return this;
    }

    public Mask RevealRange(int start, int length)
    {
        int end = Math.Min(_reveal.Length, start + length);

        for (int i = Math.Max(0, start); i < end; i++)
            _reveal[i] = true;

        return this;
    }

    public Mask RevealRegex(string pattern, RegexOptions options = RegexOptions.None)
    {
        foreach (Match m in Regex.Matches(_input, pattern, options))
        {
            for (int i = m.Index; i < m.Index + m.Length; i++)
                _reveal[i] = true;
        }

        return this;
    }

    public Mask RevealIf(Func<char, int, bool> predicate)
    {
        for (int i = 0; i < _input.Length; i++)
            if (predicate(_input[i], i))
                _reveal[i] = true;

        return this;
    }

    public override string ToString()
    {
        if (_input.Length == 0)
            return string.Empty;

        // compact mod
        if (_compactStarCount.HasValue)
        {
            var revealed = new List<(int start, int length)>();
            int i = 0;

            while (i < _input.Length)
            {
                if (_reveal[i])
                {
                    int j = i;
                    while (j < _input.Length && _reveal[j])
                        j++;
                    revealed.Add((i, j - i));
                    i = j;
                }
                else
                    i++;
            }

            // sadece ilk ve son reveal için özel kural
            if (revealed.Count == 1 && revealed[0].start == 0)
            {
                // baştan açıldı
                return string.Concat(
                    _input.AsSpan(0, revealed[0].length),
                    new string(_maskChar, _compactStarCount.Value)
                );
            }
            if (revealed.Count == 1 && revealed[0].start + revealed[0].length == _input.Length)
            {
                // sondan açıldı
                return string.Concat(
                    new string(_maskChar, _compactStarCount.Value),
                    _input.AsSpan(revealed[0].start, revealed[0].length)
                );
            }

            // range: ortada açık, hem önünde hem arkasında yıldız
            if (revealed.Count == 1)
            {
                return string.Concat(
                    new string(_maskChar, _compactStarCount.Value),
                    _input.AsSpan(revealed[0].start, revealed[0].length),
                    new string(_maskChar, _compactStarCount.Value)
                );
            }
        }

        // normal mod
        var sb = new StringBuilder(_input.Length);

        for (int i = 0; i < _input.Length; i++)
        {
            char ch = _input[i];
            if (_preserve.Contains(ch))
                sb.Append(ch);
            else if (_reveal[i])
                sb.Append(ch);
            else
                sb.Append(_maskChar);
        }

        return sb.ToString();
    }
}
