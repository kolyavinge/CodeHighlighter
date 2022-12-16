using System.Linq;
using CodeHighlighter.Common;

namespace CodeHighlighter.Model;

public enum CursorPositionKind { Real, Virtual }

public readonly struct CursorPosition
{
    public readonly int LineIndex;
    public readonly int ColumnIndex;
    public readonly CursorPositionKind Kind;

    public CursorPosition(int lineIndex, int columnIndex)
    {
        LineIndex = lineIndex;
        ColumnIndex = columnIndex;
        Kind = CursorPositionKind.Real;
    }

    internal CursorPosition(int lineIndex, int columnIndex, CursorPositionKind kind)
    {
        LineIndex = lineIndex;
        ColumnIndex = columnIndex;
        Kind = kind;
    }

    public override string ToString() => Kind == CursorPositionKind.Real ? $"{LineIndex}:{ColumnIndex}" : $"[{LineIndex}:{ColumnIndex}]";
}

internal interface ITextCursor
{
    int ColumnIndex { get; }
    CursorPositionKind Kind { get; set; }
    int LineIndex { get; }
    CursorPosition Position { get; }
    void MoveDown();
    void MoveEndLine();
    void MoveLeft();
    void MovePageDown(int pageSize);
    void MovePageUp(int pageSize);
    void MoveRight();
    void MoveStartLine();
    void MoveTextBegin();
    void MoveTextEnd();
    void MoveTo(CursorPosition position);
    void MoveUp();
}

internal class TextCursor : ITextCursor
{
    private readonly IText _text;

    public int LineIndex { get; private set; }

    public int ColumnIndex { get; private set; }

    public CursorPositionKind Kind { get; set; }

    public CursorPosition Position => new(LineIndex, ColumnIndex, Kind);

    public TextCursor(IText text)
    {
        _text = text;
        LineIndex = 0;
    }

    public void MoveTo(CursorPosition position)
    {
        LineIndex = position.LineIndex;
        ColumnIndex = position.ColumnIndex;
        Kind = position.Kind;
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
        if (Kind == CursorPositionKind.Real)
        {
            ColumnIndex--;
            if (ColumnIndex == -1)
            {
                LineIndex--;
                ColumnIndex = int.MaxValue;
            }
        }
        else
        {
            ColumnIndex = 0;
        }
        CorrectPosition();
    }

    public void MoveRight()
    {
        if (LineIndex == _text.LinesCount - 1 && ColumnIndex == _text.GetLine(LineIndex).Length) return;
        if (Kind == CursorPositionKind.Real) ColumnIndex++;
        if (Kind == CursorPositionKind.Virtual || ColumnIndex == _text.GetLine(LineIndex).Length + 1)
        {
            LineIndex++;
            ColumnIndex = 0;
        }
        CorrectPosition();
    }

    public void MoveStartLine()
    {
        var line = _text.GetLine(LineIndex);
        var spacesCount = line.FindIndex(0, line.Length, ch => ch != ' ');
        if (spacesCount == -1) ColumnIndex = 0;
        else if (ColumnIndex > spacesCount) ColumnIndex = spacesCount;
        else if (ColumnIndex == spacesCount) ColumnIndex = 0;
        else ColumnIndex = spacesCount;
        CorrectPosition();
    }

    public void MoveEndLine()
    {
        ColumnIndex = Int32.MaxValue;
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
        CorrectPosition();
    }

    public void MoveTextEnd()
    {
        LineIndex = _text.LinesCount - 1;
        ColumnIndex = _text.GetLine(LineIndex).Length;
        CorrectPosition();
    }

    private void CorrectPosition()
    {
        if (LineIndex < 0) LineIndex = 0;
        else if (LineIndex >= _text.LinesCount) LineIndex = _text.LinesCount - 1;
        var lineLength = _text.GetLine(LineIndex).Length;
        Kind = CursorPositionKind.Real;
        if (lineLength == 0 && ColumnIndex > 0 && LineIndex > 0)
        {
            var prevLineIndex = LineIndex - 1;
            var prevLine = _text.GetLine(prevLineIndex);
            while (!prevLine.Any() && prevLineIndex > 0) prevLine = _text.GetLine(--prevLineIndex);
            var spacesCount = prevLine.FindIndex(0, prevLine.Length, ch => ch != ' ');
            if (spacesCount != -1)
            {
                ColumnIndex = spacesCount;
                if (ColumnIndex > 0) Kind = CursorPositionKind.Virtual;
            }
            else
            {
                ColumnIndex = 0;
            }
        }
        else
        {
            if (ColumnIndex < 0) ColumnIndex = 0;
            else if (ColumnIndex > lineLength) ColumnIndex = lineLength;
        }
    }
}

internal static class TextCursorExt
{
    public static Point GetAbsolutePosition(this ITextCursor cursor, ITextMeasuresInternal measures) =>
        new(cursor.ColumnIndex * measures.LetterWidth, cursor.LineIndex * measures.LineHeight);
}
