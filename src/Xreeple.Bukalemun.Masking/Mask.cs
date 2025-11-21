using System.Text;
using System.Text.RegularExpressions;

namespace Xreeple.Bukalemun.Masking;

/// <summary>
/// Initializes a new instance of the <see cref="Mask"/> class with the specified input string.
/// All characters are initially hidden (masked), and no characters are preserved by default.
/// </summary>
public sealed class Mask
{
    private readonly string _input;
    private readonly bool[] _reveal;
    private readonly HashSet<char> _preserve;
    private char _maskChar = '*';
    private int? _compactStarCount = null;
    private bool _removeMasked = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mask"/> class with the specified input string.
    /// All characters are initially hidden (masked), and no characters are preserved by default.
    /// </summary>
    /// <param name="input">The input string to be masked. If null, an empty string is used.</param>
    private Mask(string input)
    {
        _input = input ?? string.Empty;
        _reveal = new bool[_input.Length];
        _preserve = [];
    }

    /// <summary>
    /// Creates a new <see cref="Mask"/> instance using the specified input string.
    /// </summary>
    /// <param name="input">The input string to be masked.</param>
    /// <returns>A new instance of the <see cref="Mask"/> class.</returns>
    public static Mask Build(string input) => new(input);

    /// <summary>
    /// Sets the character that will be used to mask unrevealed characters in the input string.
    /// </summary>
    /// <param name="c">The masking character to use (e.g., '*', '#').</param>
    /// <returns>The current <see cref="Mask"/> instance for method chaining.</returns>
    public Mask MaskChar(char c)
    {
        _maskChar = c;
        return this;
    }

    /// <summary>
    /// Enables compact masking by replacing each sequence of masked characters with a fixed number of mask characters.
    /// </summary>
    /// <param name="starCount">The number of mask characters to display in place of any masked segment (must be greater than 0).</param>
    /// <returns>The current <see cref="Mask"/> instance for method chaining.</returns>
    public Mask CompactMask(int starCount)
    {
        if (starCount > 0)
            _compactStarCount = starCount;

        return this;
    }

    /// <summary>
    /// Configures whether the mask should be removed and returns the current instance.
    /// </summary>
    /// <param name="remove">A value indicating whether the mask should be removed. The default is <see langword="true"/>.</param>
    /// <returns>The current instance of the <see cref="Mask"/> class, allowing for method chaining.</returns>
    public Mask RemoveMasked(bool remove = true)
    {
        _removeMasked = remove;
        return this;
    }

    /// <summary>
    /// Specifies characters that should always be preserved (i.e., not masked), regardless of reveal rules.
    /// </summary>
    /// <param name="chars">A string containing characters to preserve.</param>
    /// <returns>The current <see cref="Mask"/> instance for method chaining.</returns>
    public Mask PreserveChars(string chars)
    {
        if (!string.IsNullOrEmpty(chars))
            foreach (var ch in chars)
                _preserve.Add(ch);

        return this;
    }

    /// <summary>
    /// Preserves whitespace characters (space and tab) so they are not masked in the output.
    /// </summary>
    /// <returns>The current <see cref="Mask"/> instance for method chaining.</returns>
    public Mask PreserveWhitespace()
    {
        _preserve.Add(' ');
        _preserve.Add('\t');

        return this;
    }

    /// <summary>
    /// Reveals the first <paramref name="n"/> characters of the input string, keeping them unmasked.
    /// </summary>
    /// <param name="n">The number of characters to reveal from the beginning of the string.</param>
    /// <returns>The current <see cref="Mask"/> instance for method chaining.</returns>
    public Mask RevealFirst(int n)
    {
        for (int i = 0; i < Math.Min(n, _reveal.Length); i++)
            _reveal[i] = true;

        return this;
    }

    /// <summary>
    /// Reveals the last <paramref name="n"/> characters of the input string, keeping them unmasked.
    /// </summary>
    /// <param name="n">The number of characters to reveal from the end of the string.</param>
    /// <returns>The current <see cref="Mask"/> instance for method chaining.</returns>
    public Mask RevealLast(int n)
    {
        int len = _reveal.Length;

        for (int i = Math.Max(0, len - n); i < len; i++)
            _reveal[i] = true;

        return this;
    }

    /// <summary>
    /// Reveals a range of characters in the input string starting at the specified index.
    /// </summary>
    /// <param name="start">The zero-based starting index of the range to reveal.</param>
    /// <param name="length">The number of characters to reveal from the starting index.</param>
    /// <returns>The current <see cref="Mask"/> instance for method chaining.</returns>
    public Mask RevealRange(int start, int length)
    {
        int end = Math.Min(_reveal.Length, start + length);

        for (int i = Math.Max(0, start); i < end; i++)
            _reveal[i] = true;

        return this;
    }

    /// <summary>
    /// Reveals all characters in the input string that match the specified regular expression pattern.
    /// </summary>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="options">Optional regex options for matching.</param>
    /// <returns>The current <see cref="Mask"/> instance for method chaining.</returns>
    public Mask RevealRegex(string pattern, RegexOptions options = RegexOptions.None)
    {
        foreach (Match m in Regex.Matches(_input, pattern, options))
        {
            for (int i = m.Index; i < m.Index + m.Length; i++)
                _reveal[i] = true;
        }

        return this;
    }

    /// <summary>
    /// Reveals the first <paramref name="n"/> characters of each word in the input string.
    /// Words are separated by whitespace characters.
    /// </summary>
    /// <param name="n">The number of characters to reveal from the start of each word.</param>
    /// <returns>The current <see cref="Mask"/> instance for method chaining.</returns>
    public Mask RevealInitialsPerWord(int n)
    {
        if (n <= 0)
            return this;

        bool newWord = true;
        int remaining = 0;

        for (int i = 0; i < _input.Length; i++)
        {
            char current = _input[i];

            if (char.IsWhiteSpace(current))
            {
                newWord = true;
                remaining = 0;
                continue;
            }

            if (newWord && char.IsLetterOrDigit(current))
            {
                remaining = n;
                newWord = false;
            }

            if (remaining > 0)
            {
                _reveal[i] = true;
                remaining--;
            }
        }

        return this;
    }

    /// <summary>
    /// Reveals characters in the input string that satisfy the specified predicate function.
    /// </summary>
    /// <param name="predicate">A function that takes a character and its index, returning true if the character should be revealed.</param>
    /// <returns>The current <see cref="Mask"/> instance for method chaining.</returns>
    public Mask RevealIf(Func<char, int, bool> predicate)
    {
        for (int i = 0; i < _input.Length; i++)
            if (predicate(_input[i], i))
                _reveal[i] = true;

        return this;
    }

    /// <summary>
    /// Returns the masked string according to the reveal and preserve rules.
    /// If compact masking is enabled, consecutive masked characters are replaced with a fixed number of mask characters.
    /// </summary>
    /// <returns>The masked string.</returns>
    public override string ToString()
    {
        if (_input.Length == 0)
            return string.Empty;

        if (_removeMasked)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _input.Length; i++)
            {
                if (_reveal[i])
                    sb.Append(_input[i]);
            }
            return sb.ToString();
        }
        else if (_compactStarCount.HasValue)
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
