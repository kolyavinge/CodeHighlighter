using System.Windows;

namespace CodeHighlighter.Model;

internal interface ITextCursor
{
    int LineIndex { get; }
    int ColumnIndex { get; }
}

public class TextCursor : ITextCursor
{
    private readonly IText _text;

    public int LineIndex { get; private set; }

    public int ColumnIndex { get; private set; }

    internal (int, int) LineAndColumnIndex => (LineIndex, ColumnIndex);

    internal TextCursor(IText text)
    {
        _text = text;
        LineIndex = 0;
    }

    internal void MoveTo(int lineIndex, int columnIndex)
    {
        LineIndex = lineIndex;
        ColumnIndex = columnIndex;
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
    public static Point GetAbsolutePosition(this ITextCursor textCursor, TextMeasures textMeasures)
    {
        return new(textCursor.ColumnIndex * textMeasures.LetterWidth, textCursor.LineIndex * textMeasures.LineHeight);
    }
}
