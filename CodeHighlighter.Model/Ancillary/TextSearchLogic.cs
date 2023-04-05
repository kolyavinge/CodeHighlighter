using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Core;

namespace CodeHighlighter.Ancillary;

internal class TextSearchLogic : ISearchLogic
{
    public IEnumerable<TextPosition> DoSearch(IText text, string pattern, SearchOptions options)
    {
        if (String.IsNullOrWhiteSpace(pattern)) yield break;
        if (pattern.IndexOfAny(new[] { '\r', '\n' }) != -1) throw new ArgumentException("String pattern cannot be multiline.");
        var firstPatternChar = pattern.First();
        var lastPatternChar = pattern.Last();
        for (int lineIndex = 0; lineIndex < text.LinesCount; lineIndex++)
        {
            var line = text.GetLine(lineIndex);
            var length = line.Length - pattern.Length + 1;
            if (options.IgnoreCase == false)
            {
                for (int lineCol = 0; lineCol < length; lineCol++)
                {
                    if (line[lineCol] == firstPatternChar &&
                        line[lineCol + pattern.Length - 1] == lastPatternChar &&
                        IsMiddleEqual(line, lineCol, pattern))
                    {
                        yield return new(lineIndex, lineCol, lineIndex, lineCol + pattern.Length);
                    }
                }
            }
            else
            {
                for (int lineCol = 0; lineCol < length; lineCol++)
                {
                    if (CharUtils.IsCharEqualIgnoreCase(line[lineCol], firstPatternChar) &&
                        CharUtils.IsCharEqualIgnoreCase(line[lineCol + pattern.Length - 1], lastPatternChar) &&
                        IsMiddleEqualIgnoreCase(line, lineCol, pattern))
                    {
                        yield return new(lineIndex, lineCol, lineIndex, lineCol + pattern.Length);
                    }
                }
            }
        }
    }

    private bool IsMiddleEqual(TextLine line, int lineCol, string pattern)
    {
        for (int i = 1; i < pattern.Length - 1; i++)
        {
            if (line[lineCol + i] != pattern[i]) return false;
        }

        return true;
    }

    private bool IsMiddleEqualIgnoreCase(TextLine line, int lineCol, string pattern)
    {
        for (int i = 1; i < pattern.Length - 1; i++)
        {
            if (!CharUtils.IsCharEqualIgnoreCase(line[lineCol + i], pattern[i])) return false;
        }

        return true;
    }
}
