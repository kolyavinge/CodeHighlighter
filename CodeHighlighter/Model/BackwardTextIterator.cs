namespace CodeHighlighter.Model;

internal class BackwardTextIterator : ITextIterator
{
    private readonly IText _text;

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

    public BackwardTextIterator(IText text) : this(text, 0, text.LinesCount - 1)
    {
    }

    public BackwardTextIterator(IText text, int startLineIndex, int endLineIndex) : this(text, startLineIndex, text.GetLine(text.LinesCount - 1).Length, endLineIndex)
    {
    }

    public BackwardTextIterator(IText text, int startLineIndex, int startColumnIndex, int endLineIndex)
    {
        _text = text;
        LineIndex = endLineIndex;
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
        columnIndex--;
        var line = _text.GetLine(lineIndex);
        if (columnIndex > -1)
        {
            return (line[columnIndex], lineIndex, columnIndex);
        }
        else if (columnIndex == -1 && lineIndex > 0)
        {
            lineIndex--;
            columnIndex = _text.GetLine(lineIndex).Length;
            return ('\n', lineIndex, columnIndex);
        }
        else
        {
            return ((char)0, lineIndex, columnIndex);
        }
    }
}
