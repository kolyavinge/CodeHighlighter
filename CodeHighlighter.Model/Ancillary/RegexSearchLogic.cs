using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeHighlighter.Core;

namespace CodeHighlighter.Ancillary;

internal class RegexSearchLogic : ISearchLogic
{
    private int _startLineIndex;
    private int _currentCharIndex;

    public IEnumerable<TextPosition> DoSearch(IText text, string pattern, SearchOptions options)
    {
        if (String.IsNullOrWhiteSpace(pattern)) yield break;
        var textString = text.ToString();
        var regexOptions = RegexOptions.None;
        if (options.IgnoreCase) regexOptions |= RegexOptions.IgnoreCase;
        if (pattern.IndexOfAny(new[] { '\r', '\n' }) != -1) regexOptions |= RegexOptions.Multiline;
        var regex = new Regex(pattern, regexOptions);
        var matches = regex.Matches(textString);
        _startLineIndex = 0;
        _currentCharIndex = 0;
        foreach (Match match in matches)
        {
            yield return MakeSearchEntry(text, match);
        }
    }

    private TextPosition MakeSearchEntry(IText text, Match match)
    {
        int matchCharIndex = match.Index;
        int newLineLength = Environment.NewLine.Length;

        // start position
        while (true)
        {
            var line = text.GetLine(_startLineIndex);
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
            var line = text.GetLine(endLineIndex);
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
}
