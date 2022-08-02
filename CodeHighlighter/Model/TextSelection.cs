using System.Collections.Generic;

namespace CodeHighlighter.Model;

internal interface ITextSelection
{
    bool IsExist { get; }
    int StartCursorLineIndex { get; }
    int StartCursorColumnIndex { get; }
    int EndCursorLineIndex { get; }
    int EndCursorColumnIndex { get; }
    CursorPosition StartPosition { get; }
    CursorPosition EndPosition { get; }
    (CursorPosition, CursorPosition) GetSortedPositions();
    IEnumerable<TextSelectionLine> GetSelectedLines(IText text);
}

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

class TextSelection : ITextSelection
{
    public bool IsExist => StartCursorLineIndex != EndCursorLineIndex || StartCursorColumnIndex != EndCursorColumnIndex;
    public bool InProgress { get; set; }
    public int StartCursorLineIndex { get; set; }
    public int StartCursorColumnIndex { get; set; }
    public int EndCursorLineIndex { get; set; }
    public int EndCursorColumnIndex { get; set; }

    public CursorPosition StartPosition => new(StartCursorLineIndex, StartCursorColumnIndex);
    public CursorPosition EndPosition => new(EndCursorLineIndex, EndCursorColumnIndex);

    public TextSelection(int startCursorLineIndex, int startColumnIndex, int endCursorLineIndex, int endColumnIndex)
    {
        StartCursorLineIndex = startCursorLineIndex;
        StartCursorColumnIndex = startColumnIndex;
        EndCursorLineIndex = endCursorLineIndex;
        EndCursorColumnIndex = endColumnIndex;
    }

    public (CursorPosition, CursorPosition) GetSortedPositions()
    {
        var start = new CursorPosition(StartCursorLineIndex, StartCursorColumnIndex);
        var end = new CursorPosition(EndCursorLineIndex, EndCursorColumnIndex);
        if (start.LineIndex < end.LineIndex) return (start, end);
        if (start.LineIndex > end.LineIndex) return (end, start);
        if (start.ColumnIndex < end.ColumnIndex) return (start, end);
        return (end, start);
    }

    public IEnumerable<TextSelectionLine> GetSelectedLines(IText text)
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

    public void Reset()
    {
        InProgress = false;
        StartCursorLineIndex = 0;
        StartCursorColumnIndex = 0;
        EndCursorLineIndex = 0;
        EndCursorColumnIndex = 0;
    }
}
