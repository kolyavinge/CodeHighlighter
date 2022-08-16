using System;
using CodeHighlighter.CodeProvidering;

namespace CodeHighlighter.Model;

internal class BackwardTextIterator : ITextIterator
{
    private readonly IText _text;
    private TextLine _currentLine;
    private int _currentLineLength;
    private char _char;
    private int _lineIndex;
    private int _columnIndex;
    private bool _eof;

    public char Char => _char;

    public int LineIndex => _lineIndex;

    public int ColumnIndex => _columnIndex;

    public bool Eof => _eof;

    public char NextChar
    {
        get
        {
            (char nextChar, int lineIndex, int columnIndex) = GetNextCharAndPosition();
            return nextChar;
        }
    }

    public BackwardTextIterator(IText text) : this(text, 0, text.LinesCount - 1)
    {
    }

    public BackwardTextIterator(IText text, int startLineIndex, int endLineIndex) : this(text, startLineIndex, text.GetLine(text.LinesCount - 1).Length, endLineIndex)
    {
    }

    public BackwardTextIterator(IText text, int startLineIndex, int startColumnIndex, int endLineIndex)
    {
        if (startLineIndex < 0) throw new ArgumentException(nameof(startLineIndex));
        if (endLineIndex < 0) throw new ArgumentException(nameof(endLineIndex));
        if (endLineIndex < startLineIndex) throw new ArgumentException("endLineIndex must be greater then startLineIndex");
        _text = text;
        _lineIndex = endLineIndex;
        _columnIndex = startColumnIndex;
        _currentLine = _text.GetLine(_lineIndex);
        _currentLineLength = _currentLine.Length;
        MoveNext();
    }

    public void MoveNext()
    {
        (char nextChar, int lineIndex, int columnIndex) = GetNextCharAndPosition();
        _char = nextChar;
        _lineIndex = lineIndex;
        _columnIndex = columnIndex;
        _eof = _char == 0;
    }

    private (char, int, int) GetNextCharAndPosition()
    {
        var lineIndex = _lineIndex;
        var columnIndex = _columnIndex;
        if (_eof) return ((char)0, lineIndex, columnIndex);
        columnIndex--;
        if (columnIndex > -1)
        {
            return (_currentLine[columnIndex], lineIndex, columnIndex);
        }
        else if (columnIndex == -1 && lineIndex > 0)
        {
            lineIndex--;
            _currentLine = _text.GetLine(lineIndex);
            _currentLineLength = _currentLine.Length;
            columnIndex = _currentLineLength;
            return ('\n', lineIndex, columnIndex);
        }
        else
        {
            return ((char)0, lineIndex, columnIndex);
        }
    }
}
