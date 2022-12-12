using System.Collections.Generic;

namespace CodeHighlighter.Model;

public readonly struct TextSelectionLine
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
    public bool IsExist => StartPosition.LineIndex != EndPosition.LineIndex || StartPosition.ColumnIndex != EndPosition.ColumnIndex;
    internal bool InProgress { get; set; }
    public CursorPosition StartPosition { get; internal set; }
    public CursorPosition EndPosition { get; internal set; }

    public TextSelection()
    {
        Set(new(0, 0), new(0, 0));
    }

    public TextSelection(CursorPosition selectionStart, CursorPosition selectionEnd)
    {
        Set(selectionStart, selectionEnd);
    }

    public void Set(CursorPosition selectionStart, CursorPosition selectionEnd)
    {
        InProgress = false;
        StartPosition = selectionStart;
        EndPosition = selectionEnd;
    }

    internal (CursorPosition, CursorPosition) GetSortedPositions()
    {
        var start = StartPosition;
        var end = EndPosition;
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
            var rightColumnIndex = start.Kind == CursorPositionKind.Real ? text.GetLine(start.LineIndex).Length : start.ColumnIndex;
            yield return new TextSelectionLine(start.LineIndex, start.ColumnIndex, rightColumnIndex);
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
        StartPosition = new();
        EndPosition = new();
    }
}
