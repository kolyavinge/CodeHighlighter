using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Core;

namespace CodeHighlighter.Ancillary;

internal interface ITextSearchLogic
{
    IEnumerable<TextPosition> DoSearch(string pattern, bool matchCase);
}

internal class TextSearchLogic : ITextSearchLogic
{
    private readonly ITextLines _textLines;

    public TextSearchLogic(ITextLines textLines)
    {
        _textLines = textLines;
    }

    public IEnumerable<TextPosition> DoSearch(string pattern, bool matchCase)
    {
        if (pattern == "") yield break;
        if (pattern.IndexOfAny(new[] { '\r', '\n' }) != -1) throw new ArgumentException("String pattern cannot be multiline.");
        var firstPatternChar = pattern.First();
        var lastPatternChar = pattern.Last();
        for (int lineIndex = 0; lineIndex < _textLines.Count; lineIndex++)
        {
            var line = _textLines.GetLine(lineIndex);
            var length = line.Length - pattern.Length + 1;
            if (matchCase)
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

    private bool IsMiddleEqual(ITextLine line, int lineCol, string pattern)
    {
        for (int i = 1; i < pattern.Length - 1; i++)
        {
            if (line[lineCol + i] != pattern[i]) return false;
        }

        return true;
    }

    private bool IsMiddleEqualIgnoreCase(ITextLine line, int lineCol, string pattern)
    {
        for (int i = 1; i < pattern.Length - 1; i++)
        {
            if (!CharUtils.IsCharEqualIgnoreCase(line[lineCol + i], pattern[i])) return false;
        }

        return true;
    }
}
