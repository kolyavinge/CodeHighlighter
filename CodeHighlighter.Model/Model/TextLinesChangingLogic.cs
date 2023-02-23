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
}

internal interface ITextLinesChangingLogic
{
    LinesChangeResult AppendNewLine(int cursorLineIndex, int selectionStartLineIndex, int selectionEndLineIndex);
    LinesChangeResult InsertText(int insertStartLineIndex, int insertEndLineIndex, int selectionStartLineIndex, int selectionEndLineIndex);
    LinesChangeResult LeftDelete(int cursorLineIndex, bool isSelectionExist, int selectionStartLineIndex, int selectionEndLineIndex);
    LinesChangeResult RightDelete(int cursorLineIndex, bool isSelectionExist, int selectionStartLineIndex, int selectionEndLineIndex);
}

internal class TextLinesChangingLogic : ITextLinesChangingLogic
{
    public LinesChangeResult AppendNewLine(int cursorLineIndex, int selectionStartLineIndex, int selectionEndLineIndex)
    {
        var addedLines = new LinesChange(cursorLineIndex + 1, 1);
        var deletedLines = selectionStartLineIndex != selectionEndLineIndex ? GetDeleteSelection(selectionStartLineIndex, selectionEndLineIndex) : default;

        return new(addedLines, deletedLines);
    }

    public LinesChangeResult InsertText(int insertStartLineIndex, int insertEndLineIndex, int selectionStartLineIndex, int selectionEndLineIndex)
    {
        var addedLines = new LinesChange(insertStartLineIndex + 1, insertEndLineIndex - insertStartLineIndex);
        var deletedLines = selectionStartLineIndex != selectionEndLineIndex ? GetDeleteSelection(selectionStartLineIndex, selectionEndLineIndex) : default;

        return new(addedLines, deletedLines);
    }

    public LinesChangeResult LeftDelete(int cursorLineIndex, bool isSelectionExist, int selectionStartLineIndex, int selectionEndLineIndex)
    {
        LinesChange deletedLines;
        if (isSelectionExist)
        {
            deletedLines = GetDeleteSelection(selectionStartLineIndex, selectionEndLineIndex);
        }
        else
        {
            deletedLines = new LinesChange(cursorLineIndex, 1);
        }

        return new(default, deletedLines);
    }

    public LinesChangeResult RightDelete(int cursorLineIndex, bool isSelectionExist, int selectionStartLineIndex, int selectionEndLineIndex)
    {
        LinesChange deletedLines;
        if (isSelectionExist)
        {
            deletedLines = GetDeleteSelection(selectionStartLineIndex, selectionEndLineIndex);
        }
        else
        {
            deletedLines = new LinesChange(cursorLineIndex + 1, 1);
        }

        return new(default, deletedLines);
    }

    private LinesChange GetDeleteSelection(int selectionStartLineIndex, int selectionEndLineIndex)
    {
        if (selectionStartLineIndex != selectionEndLineIndex)
        {
            return new(selectionStartLineIndex + 1, selectionEndLineIndex - selectionStartLineIndex);
        }
        else
        {
            return default;
        }
    }
}
