namespace CodeHighlighter.Core;

internal interface IPointInTextSelection
{
    bool Check(CursorPosition pos);
}

internal class PointInTextSelection : IPointInTextSelection
{
    private readonly ITextSelection _textSelection;

    public PointInTextSelection(ITextSelection textSelection)
    {
        _textSelection = textSelection;
    }

    public bool Check(CursorPosition pos)
    {
        (var start, var end) = _textSelection.GetSortedPositions();

        if (start.LineIndex < pos.LineIndex && pos.LineIndex < end.LineIndex)
        {
            return true;
        }

        if (pos.LineIndex == start.LineIndex)
        {
            return start.ColumnIndex <= pos.ColumnIndex;
        }
        else if (pos.LineIndex == end.LineIndex)
        {
            return pos.ColumnIndex <= end.ColumnIndex;
        }

        return false;
    }
}
