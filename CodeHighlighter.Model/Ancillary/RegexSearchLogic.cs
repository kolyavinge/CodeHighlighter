using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeHighlighter.Core;

namespace CodeHighlighter.Ancillary;

internal interface IRegexSearchLogic
{
    IEnumerable<TextPosition> DoSearch(string pattern, bool matchCase);
}

internal class RegexSearchLogic : IRegexSearchLogic
{
    private int _startLineIndex;
    private int _currentCharIndex;
    private readonly ITextLines _textLines;

    public RegexSearchLogic(ITextLines textLines)
    {
        _textLines = textLines;
    }

    public IEnumerable<TextPosition> DoSearch(string pattern, bool matchCase)
    {
        if (pattern == "") yield break;
        var textString = _textLines.ToString();
        var regexOptions = RegexOptions.None;
        if (!matchCase) regexOptions |= RegexOptions.IgnoreCase;
        if (pattern.IndexOfAny(new[] { '\r', '\n' }) != -1) regexOptions |= RegexOptions.Multiline;
        var regex = MakeRegexOrNull(pattern, regexOptions);
        if (regex is null) yield break;
        var matches = regex.Matches(textString);
        _startLineIndex = 0;
        _currentCharIndex = 0;
        foreach (Match match in matches)
        {
            yield return MakeSearchEntry(match);
        }
    }

    private TextPosition MakeSearchEntry(Match match)
    {
        int matchCharIndex = match.Index;
        int newLineLength = Environment.NewLine.Length;

        // start position
        while (true)
        {
            var line = _textLines.GetLine(_startLineIndex);
            var lineLength = line.Length + newLineLength;
            if (_currentCharIndex + lineLength <= matchCharIndex)
            {
                _currentCharIndex += lineLength;
                _startLineIndex++;
            }
            else break;
        }
        int startColumnIndex = matchCharIndex - _currentCharIndex;

        // end position
        int endLineIndex = _startLineIndex;
        matchCharIndex = match.Index + match.Length;
        while (_currentCharIndex <= matchCharIndex)
        {
            var line = _textLines.GetLine(endLineIndex);
            var lineLength = line.Length + newLineLength;
            if (_currentCharIndex + lineLength <= matchCharIndex)
            {
                _currentCharIndex += lineLength;
                endLineIndex++;
            }
            else break;
        }
        int endColumnIndex = matchCharIndex - _currentCharIndex;

        return new(_startLineIndex, startColumnIndex, endLineIndex, endColumnIndex);
    }

    private Regex? MakeRegexOrNull(string pattern, RegexOptions regexOptions)
    {
        try
        {
            return new Regex(pattern, regexOptions);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }
}
