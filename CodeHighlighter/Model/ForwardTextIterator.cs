namespace CodeHighlighter.Model;

internal class ForwardTextIterator : ITextIterator
{
    private readonly IText _text;
    private readonly int _endLineIndex;

    public char Char { get; private set; }

    public int LineIndex { get; private set; }

    public int ColumnIndex { get; private set; }

    public bool Eof { get; private set; }

    public char NextChar
    {
        get
        {
            (char nextChar, int lineIndex, int columnIndex) = GetNextCharAndPosition();
            return nextChar;
        }
    }

    private bool IsReturn => Char == '\n';

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
        LineIndex = startLineIndex;
        ColumnIndex = startColumnIndex;
        if (endLineIndex - startLineIndex >= 0)
        {
            MoveNext();
        }
        else
        {
            Eof = true;
        }
    }

    public void MoveNext()
    {
        (char nextChar, int lineIndex, int columnIndex) = GetNextCharAndPosition();
        Char = nextChar;
        LineIndex = lineIndex;
        ColumnIndex = columnIndex;
        Eof = Char == 0;
    }

    private (char, int, int) GetNextCharAndPosition()
    {
        var lineIndex = LineIndex;
        var columnIndex = ColumnIndex;
        if (Eof) return ((char)0, lineIndex, columnIndex);
        if (IsReturn)
        {
            lineIndex++;
            columnIndex = 0;
        }
        else
        {
            columnIndex++;
        }
        var line = _text.GetLine(lineIndex);
        if (columnIndex < line.Length)
        {
            return (line[columnIndex], lineIndex, columnIndex);
        }
        else if (columnIndex == line.Length && lineIndex < _endLineIndex)
        {
            return ('\n', lineIndex, columnIndex);
        }
        else
        {
            return ((char)0, lineIndex, columnIndex);
        }
    }
}
