using System.Windows;

namespace CodeHighlighter.Model;

public readonly struct CursorPosition
{
    public readonly int LineIndex;
    public readonly int ColumnIndex;

    public CursorPosition(int lineIndex, int columnIndex)
    {
        LineIndex = lineIndex;
        ColumnIndex = columnIndex;
    }

    public override string ToString() => $"{LineIndex}:{ColumnIndex}";
}

public class TextCursor
{
    private readonly IText _text;

    public int LineIndex { get; private set; }

    public int ColumnIndex { get; private set; }

    public CursorPosition Position => new(LineIndex, ColumnIndex);

    internal TextCursor(IText text)
    {
        _text = text;
        LineIndex = 0;
    }

    internal void MoveTo(CursorPosition position)
    {
        LineIndex = position.LineIndex;
        ColumnIndex = position.ColumnIndex;
        CorrectPosition();
    }

    internal void MoveUp()
    {
        LineIndex--;
        CorrectPosition();
    }

    internal void MoveDown()
    {
        LineIndex++;
        CorrectPosition();
    }

    internal void MoveLeft()
    {
        if (LineIndex == 0 && ColumnIndex == 0) return;
        ColumnIndex--;
        if (ColumnIndex == -1)
        {
            LineIndex--;
            ColumnIndex = int.MaxValue;
        }
        CorrectPosition();
    }

    internal void MoveRight()
    {
        if (LineIndex == _text.LinesCount - 1 && ColumnIndex == _text.GetLine(LineIndex).Length) return;
        ColumnIndex++;
        if (ColumnIndex == _text.GetLine(LineIndex).Length + 1)
        {
            LineIndex++;
            ColumnIndex = 0;
        }
        CorrectPosition();
    }

    internal void MoveStartLine()
    {
        ColumnIndex = 0;
        CorrectPosition();
    }

    internal void MoveEndLine()
    {
        ColumnIndex = _text.GetLine(LineIndex).Length;
        CorrectPosition();
    }

    internal void MovePageUp(int pageSize)
    {
        LineIndex -= pageSize;
        CorrectPosition();
    }

    internal void MovePageDown(int pageSize)
    {
        LineIndex += pageSize;
        CorrectPosition();
    }

    internal void MoveTextBegin()
    {
        ColumnIndex = 0;
        LineIndex = 0;
    }

    internal void MoveTextEnd()
    {
        LineIndex = _text.LinesCount - 1;
        ColumnIndex = _text.GetLine(LineIndex).Length;
    }

    private void CorrectPosition()
    {
        if (LineIndex < 0) LineIndex = 0;
        if (LineIndex >= _text.LinesCount) LineIndex = _text.LinesCount - 1;
        if (ColumnIndex < 0) ColumnIndex = 0;
        if (ColumnIndex > _text.GetLine(LineIndex).Length) ColumnIndex = _text.GetLine(LineIndex).Length;
    }
}

internal static class TextCursorExt
{
    public static Point GetAbsolutePosition(this TextCursor cursor, TextMeasures measures) =>
        new(cursor.ColumnIndex * measures.LetterWidth, cursor.LineIndex * measures.LineHeight);
}
