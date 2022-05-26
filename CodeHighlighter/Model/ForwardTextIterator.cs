namespace CodeHighlighter.Model;

internal class ForwardTextIterator : ITextIterator
{
    private readonly IText _text;
    private readonly int _endLineIndex;
    private TextLine _currentLine;
    private int _currentLineLength;
    private char _char;
    private int _lineIndex;
    private int _columnIndex;
    private bool _eof;
    private bool _isReturn;

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

    public ForwardTextIterator(IText text) : this(text, 0, text.LinesCount - 1)
    {
    }

    public ForwardTextIterator(IText text, int startLineIndex, int endLineIndex) : this(text, startLineIndex, -1, endLineIndex)
    {
    }

    public ForwardTextIterator(IText text, int startLineIndex, int startColumnIndex, int endLineIndex)
    {
        _text = text;
        _endLineIndex = endLineIndex;
        _lineIndex = startLineIndex;
        _columnIndex = startColumnIndex;
        _currentLine = _text.GetLine(_lineIndex);
        _currentLineLength = _currentLine.Length;
        if (endLineIndex - startLineIndex >= 0)
        {
            MoveNext();
        }
        else
        {
            _eof = true;
        }
    }

    public void MoveNext()
    {
        (char nextChar, int lineIndex, int columnIndex) = GetNextCharAndPosition();
        _char = nextChar;
        _lineIndex = lineIndex;
        _columnIndex = columnIndex;
        _eof = _char == 0;
        _isReturn = _char == '\n';
    }

    private (char, int, int) GetNextCharAndPosition()
    {
        var lineIndex = _lineIndex;
        var columnIndex = _columnIndex;
        if (_eof) return ((char)0, lineIndex, columnIndex);
        if (_isReturn)
        {
            lineIndex++;
            columnIndex = 0;
            _currentLine = _text.GetLine(lineIndex);
            _currentLineLength = _currentLine.Length;
        }
        else
        {
            columnIndex++;
        }
        if (columnIndex < _currentLineLength)
        {
            return (_currentLine[columnIndex], lineIndex, columnIndex);
        }
        else if (columnIndex == _currentLineLength && lineIndex < _endLineIndex)
        {
            return ('\n', lineIndex, columnIndex);
        }
        else
        {
            return ((char)0, lineIndex, columnIndex);
        }
    }
}
