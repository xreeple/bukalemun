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

    public Mask RevealInitialsPerWord()
    {
        bool newWord = true;

        for (int i = 0; i < _input.Length; i++)
        {
            if (char.IsLetterOrDigit(_input[i]) && newWord)
            {
                _reveal[i] = true;
                newWord = false;
            }
            else if (char.IsWhiteSpace(_input[i]))
            {
                newWord = true;
            }
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

        if (_compactStarCount.HasValue)
        {
            var sb = new StringBuilder();
            int i = 0;
            while (i < _input.Length)
            {
                if (_reveal[i])
                {
                    sb.Append(_input[i]);
                    i++;
                }
                else if (char.IsWhiteSpace(_input[i]))
                {
                    sb.Append(_input[i]);
                    i++;
                }
                else
                {
                    while (i < _input.Length && !_reveal[i] && !char.IsWhiteSpace(_input[i]))
                        i++;
                    sb.Append(new string(_maskChar, _compactStarCount.Value));
                }
            }
            return sb.ToString();
        }

        var sb2 = new StringBuilder(_input.Length);

        for (int i = 0; i < _input.Length; i++)
        {
            if (_reveal[i] || _preserve.Contains(_input[i]))
                sb2.Append(_input[i]);
            else
                sb2.Append(_maskChar);
        }

        return sb2.ToString();
    }
}
