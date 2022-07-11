namespace CodeHighlighter;

public readonly struct TextCursorPosition
{
    public readonly int LineIndex;
    public readonly int ColumnIndex;

    public TextCursorPosition(int lineIndex, int columnIndex)
    {
        LineIndex = lineIndex;
        ColumnIndex = columnIndex;
    }
}
