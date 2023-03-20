namespace CodeHighlighter.Model;

public readonly struct LinesChange
{
    public readonly int StartLineIndex;
    public readonly int LinesCount;

    public LinesChange(int startLineIndex, int linesCount)
    {
        StartLineIndex = startLineIndex;
        LinesCount = linesCount;
    }

    public override string ToString() => $"{StartLineIndex}:{LinesCount}";
}

internal readonly struct LinesChangeResult
{
    public readonly LinesChange AddedLines;
    public readonly LinesChange DeletedLines;

    public LinesChangeResult(LinesChange addedLines, LinesChange deletedLines)
    {
        AddedLines = addedLines;
        DeletedLines = deletedLines;
    }

    public override string ToString() => $"AddedLines {AddedLines}, DeletedLines {DeletedLines}";
}

internal interface ITextLinesChangingLogic
{
    LinesChangeResult AppendNewLine(CursorPosition cursorStart, CursorPosition selectionStart, CursorPosition selectionEnd);
    LinesChangeResult InsertText(CursorPosition insertStart, CursorPosition insertEnd, CursorPosition selectionStart, CursorPosition selectionEnd);
    LinesChangeResult LeftDelete(CursorPosition cursorEnd, CursorPosition selectionStart, CursorPosition selectionEnd);
    LinesChangeResult RightDelete(CursorPosition cursorStart, CursorPosition selectionStart, CursorPosition selectionEnd);
}

internal class TextLinesChangingLogic : ITextLinesChangingLogic
{
    public LinesChangeResult AppendNewLine(CursorPosition cursorStart, CursorPosition selectionStart, CursorPosition selectionEnd)
    {
        LinesChange addedLines;
        if (cursorStart.ColumnIndex == 0)
        {
            addedLines = new LinesChange(cursorStart.LineIndex, 1);
        }
        else
        {
            addedLines = new LinesChange(cursorStart.LineIndex + 1, 1);
        }
        var deletedLines = GetDeleteSelection(selectionStart, selectionEnd);

        return new(addedLines, deletedLines);
    }

    public LinesChangeResult InsertText(CursorPosition insertStart, CursorPosition insertEnd, CursorPosition selectionStart, CursorPosition selectionEnd)
    {
        LinesChange addedLines;
        if (insertStart.ColumnIndex == 0)
        {
            addedLines = new LinesChange(insertStart.LineIndex, insertEnd.LineIndex - insertStart.LineIndex);
        }
        else
        {
            addedLines = new LinesChange(insertStart.LineIndex + 1, insertEnd.LineIndex - insertStart.LineIndex);
        }
        var deletedLines = GetDeleteSelection(selectionStart, selectionEnd);

        return new(addedLines, deletedLines);
    }

    public LinesChangeResult LeftDelete(CursorPosition cursorEnd, CursorPosition selectionStart, CursorPosition selectionEnd)
    {
        LinesChange deletedLines;
        if (!selectionStart.Equals(selectionEnd))
        {
            deletedLines = GetDeleteSelection(selectionStart, selectionEnd);
        }
        else if (cursorEnd.ColumnIndex == 0)
        {
            deletedLines = new LinesChange(cursorEnd.LineIndex, 1);
        }
        else
        {
            deletedLines = new LinesChange(cursorEnd.LineIndex + 1, 1);
        }

        return new(default, deletedLines);
    }

    public LinesChangeResult RightDelete(CursorPosition cursorStart, CursorPosition selectionStart, CursorPosition selectionEnd)
    {
        LinesChange deletedLines;
        if (!selectionStart.Equals(selectionEnd))
        {
            deletedLines = GetDeleteSelection(selectionStart, selectionEnd);
        }
        else if (cursorStart.ColumnIndex == 0)
        {
            deletedLines = new LinesChange(cursorStart.LineIndex, 1);
        }
        else
        {
            deletedLines = new LinesChange(cursorStart.LineIndex + 1, 1);
        }

        return new(default, deletedLines);
    }

    private LinesChange GetDeleteSelection(CursorPosition selectionStart, CursorPosition selectionEnd)
    {
        if (selectionEnd.LineIndex != selectionStart.LineIndex)
        {
            if (selectionStart.ColumnIndex == 0)
            {
                return new(selectionStart.LineIndex, selectionEnd.LineIndex - selectionStart.LineIndex);
            }
            else
            {
                return new(selectionStart.LineIndex + 1, selectionEnd.LineIndex - selectionStart.LineIndex);
            }
        }
        else
        {
            return default;
        }
    }
}
