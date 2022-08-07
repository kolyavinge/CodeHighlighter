using System.Collections.Generic;

namespace CodeHighlighter.Model;

internal readonly struct TextSelectionLine
{
    public readonly int LineIndex;
    public readonly int LeftColumnIndex;
    public readonly int RightColumnIndex;

    public TextSelectionLine(int lineIndex, int leftColumnIndex, int rightColumnIndex)
    {
        LineIndex = lineIndex;
        LeftColumnIndex = leftColumnIndex;
        RightColumnIndex = rightColumnIndex;
    }
}

public class TextSelection
{
    public bool IsExist => StartCursorLineIndex != EndCursorLineIndex || StartCursorColumnIndex != EndCursorColumnIndex;
    internal bool InProgress { get; set; }
    internal int StartCursorLineIndex { get; set; }
    internal int StartCursorColumnIndex { get; set; }
    internal int EndCursorLineIndex { get; set; }
    internal int EndCursorColumnIndex { get; set; }

    public CursorPosition StartPosition => new(StartCursorLineIndex, StartCursorColumnIndex);
    public CursorPosition EndPosition => new(EndCursorLineIndex, EndCursorColumnIndex);

    internal TextSelection(int startCursorLineIndex, int startColumnIndex, int endCursorLineIndex, int endColumnIndex)
    {
        StartCursorLineIndex = startCursorLineIndex;
        StartCursorColumnIndex = startColumnIndex;
        EndCursorLineIndex = endCursorLineIndex;
        EndCursorColumnIndex = endColumnIndex;
    }

    public void Set(CursorPosition selectionStart, CursorPosition selectionEnd)
    {
        InProgress = false;
        StartCursorLineIndex = selectionStart.LineIndex;
        StartCursorColumnIndex = selectionStart.ColumnIndex;
        EndCursorLineIndex = selectionEnd.LineIndex;
        EndCursorColumnIndex = selectionEnd.ColumnIndex;
    }

    internal (CursorPosition, CursorPosition) GetSortedPositions()
    {
        var start = new CursorPosition(StartCursorLineIndex, StartCursorColumnIndex);
        var end = new CursorPosition(EndCursorLineIndex, EndCursorColumnIndex);
        if (start.LineIndex < end.LineIndex) return (start, end);
        if (start.LineIndex > end.LineIndex) return (end, start);
        if (start.ColumnIndex < end.ColumnIndex) return (start, end);
        return (end, start);
    }

    internal IEnumerable<TextSelectionLine> GetSelectedLines(IText text)
    {
        if (!IsExist) yield break;
        var (start, end) = GetSortedPositions();
        if (start.LineIndex == end.LineIndex)
        {
            yield return new TextSelectionLine(start.LineIndex, start.ColumnIndex, end.ColumnIndex);
        }
        else
        {
            yield return new TextSelectionLine(start.LineIndex, start.ColumnIndex, text.GetLine(start.LineIndex).Length);
            for (int middleLineIndex = start.LineIndex + 1; middleLineIndex <= end.LineIndex - 1; middleLineIndex++)
            {
                yield return new TextSelectionLine(middleLineIndex, 0, text.GetLine(middleLineIndex).Length);
            }
            yield return new TextSelectionLine(end.LineIndex, 0, end.ColumnIndex);
        }
    }

    internal void Reset()
    {
        InProgress = false;
        StartCursorLineIndex = 0;
        StartCursorColumnIndex = 0;
        EndCursorLineIndex = 0;
        EndCursorColumnIndex = 0;
    }
}
