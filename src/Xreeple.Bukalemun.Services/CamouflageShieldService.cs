using System.Text;
using Xreeple.Bukalemun.Services.Options;

namespace Xreeple.Bukalemun.Services
{
    public class CamouflageShieldService : Abstractions.CamouflageShieldService
    {
        public string Mask(string input, CamouflageShieldOptions options)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return options.Mode switch
            {
                CamouflageShieldMode.ShowFirstLetters => MaskShowFirstLetters(input),
                CamouflageShieldMode.MaskAllExceptSpaces => MaskAllExceptSpaces(input),
                CamouflageShieldMode.MaskFromEnd => MaskFromEnd(input, options.Count),
                CamouflageShieldMode.MaskFromStart => MaskFromStart(input, options.Count),
                CamouflageShieldMode.MaskEachWordFromStart => MaskEachWord(input, options.Count, fromStart: true),
                CamouflageShieldMode.MaskEachWordFromEnd => MaskEachWord(input, options.Count, fromStart: false),
                _ => throw new ArgumentException("Invalid mask mode"),
            };
        }

        private static string MaskShowFirstLetters(string input)
        {
            var result = new StringBuilder();
            bool newWord = true;

            foreach (var c in input)
            {
                if (char.IsWhiteSpace(c))
                {
                    result.Append(c);
                    newWord = true;
                }
                else
                {
                    if (newWord)
                    {
                        result.Append(c);
                        newWord = false;
                    }
                    else
                    {
                        result.Append('*');
                    }
                }
            }

            return result.ToString();
        }

        private static string MaskAllExceptSpaces(string input)
        {
            var result = new StringBuilder();

            foreach (var c in input)
            {
                if (char.IsWhiteSpace(c))
                    result.Append(c);
                else
                    result.Append('*');
            }

            return result.ToString();
        }

        private static string MaskFromEnd(string input, int count)
        {
            if (count <= 0) return input;

            var result = new StringBuilder();
            int nonSpaceCount = 0;

            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (!char.IsWhiteSpace(input[i]))
                    nonSpaceCount++;
            }

            int toMask = Math.Min(count, nonSpaceCount);
            int maskedCount = 0;

            for (int i = input.Length - 1; i >= 0; i--)
            {
                char c = input[i];
                if (!char.IsWhiteSpace(c) && maskedCount < toMask)
                {
                    result.Insert(0, '*');
                    maskedCount++;
                }
                else
                {
                    result.Insert(0, c);
                }
            }

            return result.ToString();
        }

        private static string MaskFromStart(string input, int count)
        {
            if (count <= 0) return input;

            var result = new StringBuilder();
            int maskedCount = 0;

            foreach (var c in input)
            {
                if (!char.IsWhiteSpace(c) && maskedCount < count)
                {
                    result.Append('*');
                    maskedCount++;
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        private static string MaskEachWord(string input, int count, bool fromStart)
        {
            if (count <= 0) return input;

            var words = input.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = fromStart
                    ? MaskWordFromStart(words[i], count)
                    : MaskWordFromEnd(words[i], count);
            }

            return string.Join(" ", words);
        }

        private static string MaskWordFromStart(string word, int count)
        {
            if (string.IsNullOrEmpty(word) || count <= 0)
                return word;

            int maskCount = Math.Min(count, word.Length);
            return string.Concat(new string('*', maskCount), word.AsSpan(maskCount));
        }

        private static string MaskWordFromEnd(string word, int count)
        {
            if (string.IsNullOrEmpty(word) || count <= 0)
                return word;

            int maskCount = Math.Min(count, word.Length);
            return string.Concat(word.AsSpan(0, word.Length - maskCount), new string('*', maskCount));
        }
    }
}
