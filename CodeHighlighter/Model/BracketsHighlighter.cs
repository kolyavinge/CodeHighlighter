using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

internal class BracketsHighlighter
{
    private readonly IText _text;
    private readonly ITextCursor _textCursor;
    private readonly List<BracketPair> _bracketPairs = new();
    private HighlightResult _lastResult;

    public BracketsHighlighter(string bracketsString, IText text, ITextCursor textCursor)
    {
        if (bracketsString.Length % 2 != 0) throw new ArgumentException(nameof(bracketsString));
        for (int i = 0; i < bracketsString.Length; i += 2)
        {
            _bracketPairs.Add(new(bracketsString[i], bracketsString[i + 1]));
        }
        _text = text;
        _textCursor = textCursor;
    }

    public HighlightResult GetHighlightedBrackets()
    {
        if (!_bracketPairs.Any()) return new(HighlightKind.NoHighlight, default, default);

        var cursorLineIndex = _textCursor.LineIndex;
        var cursorColumnIndex = _textCursor.ColumnIndex;

        var cursorChar = (char)0;
        if (cursorColumnIndex < _text.GetLine(cursorLineIndex).Length)
        {
            cursorChar = _text.GetLine(cursorLineIndex)[cursorColumnIndex];
            if (!_bracketPairs.Any(x => x.Open == cursorChar || x.Close == cursorChar) && cursorColumnIndex > 0)
            {
                cursorColumnIndex--;
                cursorChar = _text.GetLine(cursorLineIndex)[cursorColumnIndex];
            }
        }
        else if (cursorColumnIndex > 0)
        {
            cursorColumnIndex--;
            cursorChar = _text.GetLine(cursorLineIndex)[cursorColumnIndex];
        }

        // open bracket
        if (_bracketPairs.Any(x => x.Open == cursorChar))
        {
            var closeBracket = _bracketPairs.First(x => x.Open == cursorChar).Close;
            int count = 1;
            var iter = new ForwardTextIterator(_text, cursorLineIndex, cursorColumnIndex, _text.LinesCount - 1);
            while (!iter.Eof)
            {
                if (iter.Char == cursorChar) count++;
                else if (iter.Char == closeBracket) count--;
                if (count == 0) break;
                iter.MoveNext();
            }
            if (count == 0)
            {
                _lastResult = new(HighlightKind.Highlighted, new(cursorLineIndex, cursorColumnIndex), new(iter.LineIndex, iter.ColumnIndex));
            }
            else
            {
                _lastResult = new(HighlightKind.NoPair, new(cursorLineIndex, cursorColumnIndex), new(cursorLineIndex, cursorColumnIndex));
            }
        }
        // close bracket
        else if (_bracketPairs.Any(x => x.Close == cursorChar))
        {
            var openBracket = _bracketPairs.First(x => x.Close == cursorChar).Open;
            int count = 1;
            var iter = new BackwardTextIterator(_text, 0, cursorColumnIndex, cursorLineIndex);
            while (!iter.Eof)
            {
                if (iter.Char == cursorChar) count++;
                else if (iter.Char == openBracket) count--;
                if (count == 0) break;
                iter.MoveNext();
            }
            if (count == 0)
            {
                _lastResult = new(HighlightKind.Highlighted, new(iter.LineIndex, iter.ColumnIndex), new(cursorLineIndex, cursorColumnIndex));
            }
            else
            {
                _lastResult = new(HighlightKind.NoPair, new(cursorLineIndex, cursorColumnIndex), new(cursorLineIndex, cursorColumnIndex));
            }
        }
        // no brackets selection
        else
        {
            _lastResult = new(HighlightKind.NoHighlight, default, default);
        }

        return _lastResult;
    }

    readonly struct BracketPair
    {
        public readonly char Open;
        public readonly char Close;
        public BracketPair(char open, char close)
        {
            Open = open;
            Close = close;
        }
    }

    public readonly struct BracketPosition
    {
        public readonly int LineIndex;
        public readonly int ColumnIndex;
        public BracketPosition(int lineIndex, int columnIndex)
        {
            LineIndex = lineIndex;
            ColumnIndex = columnIndex;
        }
    }

    public enum HighlightKind
    {
        NoHighlight,
        Highlighted,
        NoPair
    }

    public readonly struct HighlightResult
    {
        public readonly BracketPosition Open;
        public readonly BracketPosition Close;
        public readonly HighlightKind Kind;
        public HighlightResult(HighlightKind kind, BracketPosition open, BracketPosition close)
        {
            Open = open;
            Close = close;
            Kind = kind;
        }
    }
}
