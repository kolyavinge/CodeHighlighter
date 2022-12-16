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

public interface ITextSelection
{
    bool IsExist { get; }
    CursorPosition StartPosition { get; set; }
    CursorPosition EndPosition { get; set; }
    IEnumerable<TextSelectionLine> GetSelectedLines();
}

internal interface ITextSelectionInternal : ITextSelection
{
    bool InProgress { get; set; }
    (CursorPosition, CursorPosition) GetSortedPositions();
    void Reset();
    ITextSelectionInternal Set(CursorPosition selectionStart, CursorPosition selectionEnd);
}

internal class TextSelection : ITextSelectionInternal
{
    private readonly IText _text;

    public bool IsExist => StartPosition.LineIndex != EndPosition.LineIndex || StartPosition.ColumnIndex != EndPosition.ColumnIndex;
    public bool InProgress { get; set; }
    public CursorPosition StartPosition { get; set; }
    public CursorPosition EndPosition { get; set; }

    public TextSelection(IText text)
    {
        _text = text;
    }

    public ITextSelectionInternal Set(CursorPosition selectionStart, CursorPosition selectionEnd)
    {
        InProgress = false;
        StartPosition = selectionStart;
        EndPosition = selectionEnd;

        return this;
    }

    public (CursorPosition, CursorPosition) GetSortedPositions()
    {
        var start = StartPosition;
        var end = EndPosition;
        if (start.LineIndex < end.LineIndex) return (start, end);
        if (start.LineIndex > end.LineIndex) return (end, start);
        if (start.ColumnIndex < end.ColumnIndex) return (start, end);
        return (end, start);
    }

    public IEnumerable<TextSelectionLine> GetSelectedLines()
    {
        if (!IsExist) yield break;
        var (start, end) = GetSortedPositions();
        if (start.LineIndex == end.LineIndex)
        {
            yield return new TextSelectionLine(start.LineIndex, start.ColumnIndex, end.ColumnIndex);
        }
        else
        {
            var rightColumnIndex = start.Kind == CursorPositionKind.Real ? _text.GetLine(start.LineIndex).Length : start.ColumnIndex;
            yield return new TextSelectionLine(start.LineIndex, start.ColumnIndex, rightColumnIndex);
            for (int middleLineIndex = start.LineIndex + 1; middleLineIndex <= end.LineIndex - 1; middleLineIndex++)
            {
                yield return new TextSelectionLine(middleLineIndex, 0, _text.GetLine(middleLineIndex).Length);
            }
            yield return new TextSelectionLine(end.LineIndex, 0, end.ColumnIndex);
        }
    }

    public void Reset()
    {
        InProgress = false;
        StartPosition = new();
        EndPosition = new();
    }
}
