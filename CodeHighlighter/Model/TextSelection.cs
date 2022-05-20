using System.Collections.Generic;

namespace CodeHighlighter.Model;

internal interface ITextSelection
{
    bool IsExist { get; }
    int StartLineIndex { get; }
    int StartCursorColumnIndex { get; }
    int EndLineIndex { get; }
    int EndCursorColumnIndex { get; }
    (int, int) StartLineAndColumnIndex { get; }
    (int, int) EndLineAndColumnIndex { get; }
    (TextSelectionPosition, TextSelectionPosition) GetSortedPositions();
    IEnumerable<TextSelectionLine> GetSelectedLines(IText text);
}

internal struct TextSelectionPosition
{
    public readonly int LineIndex;
    public readonly int ColumnIndex;

    public TextSelectionPosition(int lineIndex, int columnIndex)
    {
        LineIndex = lineIndex;
        ColumnIndex = columnIndex;
    }
}

internal struct TextSelectionLine
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

class TextSelection : ITextSelection
{
    public bool IsExist => StartLineIndex != EndLineIndex || StartCursorColumnIndex != EndCursorColumnIndex;
    public bool InProgress { get; set; }
    public int StartLineIndex { get; set; }
    public int StartCursorColumnIndex { get; set; }
    public int EndLineIndex { get; set; }
    public int EndCursorColumnIndex { get; set; }

    public (int, int) StartLineAndColumnIndex => (StartLineIndex, StartCursorColumnIndex);
    public (int, int) EndLineAndColumnIndex => (EndLineIndex, EndCursorColumnIndex);

    public TextSelection()
    {
    }

    public TextSelection(int startLineIndex, int startColumnIndex, int endLineIndex, int endColumnIndex) : this()
    {
        StartLineIndex = startLineIndex;
        StartCursorColumnIndex = startColumnIndex;
        EndLineIndex = endLineIndex;
        EndCursorColumnIndex = endColumnIndex;
    }

    public (TextSelectionPosition, TextSelectionPosition) GetSortedPositions()
    {
        var start = new TextSelectionPosition(StartLineIndex, StartCursorColumnIndex);
        var end = new TextSelectionPosition(EndLineIndex, EndCursorColumnIndex);
        if (start.LineIndex < end.LineIndex) return (start, end);
        if (start.LineIndex > end.LineIndex) return (end, start);
        if (start.ColumnIndex < end.ColumnIndex) return (start, end);
        return (end, start);
    }

    public IEnumerable<TextSelectionLine> GetSelectedLines(IText text)
    {
        if (!IsExist) yield break;
        (var start, var end) = GetSortedPositions();
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

    public void Reset()
    {
        InProgress = false;
        StartLineIndex = 0;
        StartCursorColumnIndex = 0;
        EndLineIndex = 0;
        EndCursorColumnIndex = 0;
    }
}
