using System.Windows;

namespace CodeHighlighter.Model;

internal interface ITextCursor
{
    int LineIndex { get; }
    int ColumnIndex { get; }
    Point GetAbsolutePosition(TextMeasures textMeasures);
    (int, int) GetLineAndColumnIndex { get; }
}

internal class TextCursor : ITextCursor, Contracts.ITextCursor
{
    private readonly IText _text;

    public int LineIndex { get; private set; }

    public int ColumnIndex { get; private set; }

    public Point GetAbsolutePosition(TextMeasures textMeasures) => new(ColumnIndex * textMeasures.LetterWidth, LineIndex * textMeasures.LineHeight);

    public (int, int) GetLineAndColumnIndex => (LineIndex, ColumnIndex);

    public TextCursor(IText text)
    {
        _text = text;
        LineIndex = 0;
    }

    public void MoveTo(int lineIndex, int columnIndex)
    {
        LineIndex = lineIndex;
        ColumnIndex = columnIndex;
        CorrectPosition();
    }

    public void MoveUp()
    {
        LineIndex--;
        CorrectPosition();
    }

    public void MoveDown()
    {
        LineIndex++;
        CorrectPosition();
    }

    public void MoveLeft()
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

    public void MoveRight()
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

    public void MoveStartLine()
    {
        ColumnIndex = 0;
        CorrectPosition();
    }

    public void MoveEndLine()
    {
        ColumnIndex = _text.GetLine(LineIndex).Length;
        CorrectPosition();
    }

    public void MovePageUp(int pageSize)
    {
        LineIndex -= pageSize;
        CorrectPosition();
    }

    public void MovePageDown(int pageSize)
    {
        LineIndex += pageSize;
        CorrectPosition();
    }

    public void MoveTextBegin()
    {
        ColumnIndex = 0;
        LineIndex = 0;
    }

    public void MoveTextEnd()
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
