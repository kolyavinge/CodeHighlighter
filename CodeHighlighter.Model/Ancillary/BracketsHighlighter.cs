﻿using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Core;
using CodeHighlighter.Model;
using static CodeHighlighter.Ancillary.IBracketsHighlighter;

namespace CodeHighlighter.Ancillary;

public interface IBracketsHighlighter
{
    HighlightResult GetHighlightedBrackets(CursorPosition position);

    internal readonly record struct BracketPair(char Open, char Close);

    public readonly record struct BracketPosition(int LineIndex, int ColumnIndex);

    public enum HighlightKind { NoHighlight, Highlighted, NoPair }

    public readonly record struct HighlightResult(HighlightKind Kind, BracketPosition Open, BracketPosition Close);
}

internal class BracketsHighlighter : IBracketsHighlighter
{
    private readonly List<BracketPair> _bracketPairs = new();
    private readonly IText _text;
    private HighlightResult _lastResult;

    public BracketsHighlighter(IText text, CodeTextBoxModelAdditionalParams additionalParams)
        : this(text, additionalParams.HighlighteredBrackets ?? "") { }

    internal BracketsHighlighter(IText text, string bracketsString)
    {
        if (bracketsString.Length % 2 != 0) throw new ArgumentException(nameof(bracketsString));
        for (int i = 0; i < bracketsString.Length; i += 2)
        {
            _bracketPairs.Add(new(bracketsString[i], bracketsString[i + 1]));
        }
        _text = text;
    }

    public HighlightResult GetHighlightedBrackets(CursorPosition position)
    {
        if (!_bracketPairs.Any()) return new(HighlightKind.NoHighlight, default, default);
        if (position.Kind == CursorPositionKind.Virtual) return new(HighlightKind.NoHighlight, default, default);

        var cursorLineIndex = position.LineIndex;
        var cursorColumnIndex = position.ColumnIndex;

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
}
