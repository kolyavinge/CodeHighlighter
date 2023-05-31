using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Core;

public readonly record struct TextSelectionLine(int LineIndex, int LeftColumnIndex, int RightColumnIndex);

public interface ITextSelection
{
    bool IsExist { get; }
    CursorPosition StartPosition { get; set; }
    CursorPosition EndPosition { get; set; }
    (CursorPosition, CursorPosition) GetSortedPositions();
    IEnumerable<TextSelectionLine> GetSelectedLines();
}

internal interface ITextSelectionInternal : ITextSelection
{
    bool InProgress { get; set; }
    ITextSelectionInternal Set(CursorPosition selectionStart, CursorPosition selectionEnd);
    void Reset();
}

internal class TextSelection : ITextSelectionInternal
{
    private readonly ITextSelectionLineConverter _textSelectionLineConverter;

    public bool IsExist => StartPosition.LineIndex != EndPosition.LineIndex || StartPosition.ColumnIndex != EndPosition.ColumnIndex;
    public bool InProgress { get; set; }
    public CursorPosition StartPosition { get; set; }
    public CursorPosition EndPosition { get; set; }

    public TextSelection(ITextSelectionLineConverter textSelectionLineConverter)
    {
        _textSelectionLineConverter = textSelectionLineConverter;
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
        if (!IsExist) return Enumerable.Empty<TextSelectionLine>();
        var (start, end) = GetSortedPositions();

        return _textSelectionLineConverter.GetSelectedLines(start, end);
    }

    public void Reset()
    {
        InProgress = false;
        StartPosition = new();
        EndPosition = new();
    }
}
