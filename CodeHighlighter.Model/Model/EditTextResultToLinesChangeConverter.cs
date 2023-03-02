namespace CodeHighlighter.Model;

internal interface IEditTextResultToLinesChangeConverter
{
    LinesChangeResult MakeForAppendNewLine(AppendNewLineResult textResult);
    LinesChangeResult MakeForInsertText(InsertTextResult textResult);
    LinesChangeResult MakeForLeftDelete(DeleteResult textResult);
    LinesChangeResult MakeForRightDelete(DeleteResult textResult);
    LinesChangeResult MakeForDeleteToken(DeleteTokenResult textResult);
    LinesChangeResult MakeForDeleteSelectedLines(DeleteSelectedLinesResult textResult);
}

internal class EditTextResultToLinesChangeConverter : IEditTextResultToLinesChangeConverter
{
    private readonly ITextLinesChangingLogic _logic;

    public EditTextResultToLinesChangeConverter(ITextLinesChangingLogic textLinesChangingLogic)
    {
        _logic = textLinesChangingLogic;
    }

    public LinesChangeResult MakeForAppendNewLine(AppendNewLineResult textResult)
    {
        return _logic.AppendNewLine(textResult.OldCursorPosition.LineIndex, textResult.SelectionStart.LineIndex, textResult.SelectionEnd.LineIndex);
    }

    public LinesChangeResult MakeForInsertText(InsertTextResult textResult)
    {
        if (!textResult.HasInserted) return new();
        return _logic.InsertText(
            textResult.InsertStartPosition.LineIndex, textResult.InsertEndPosition.LineIndex, textResult.SelectionStart.LineIndex, textResult.SelectionEnd.LineIndex);
    }

    public LinesChangeResult MakeForLeftDelete(DeleteResult textResult)
    {
        if (!(textResult.IsSelectionExist && textResult.HasDeleted) && !textResult.CharDeleteResult.IsLineDeleted) return new();
        return _logic.LeftDelete(textResult.OldCursorPosition.LineIndex, textResult.IsSelectionExist, textResult.SelectionStart.LineIndex, textResult.SelectionEnd.LineIndex);
    }

    public LinesChangeResult MakeForRightDelete(DeleteResult textResult)
    {
        if (!(textResult.IsSelectionExist && textResult.HasDeleted) && !textResult.CharDeleteResult.IsLineDeleted) return new();
        return _logic.RightDelete(textResult.OldCursorPosition.LineIndex, textResult.IsSelectionExist, textResult.SelectionStart.LineIndex, textResult.SelectionEnd.LineIndex);
    }

    public LinesChangeResult MakeForDeleteToken(DeleteTokenResult textResult)
    {
        if (!textResult.HasDeleted) return new();
        return _logic.LeftDelete(textResult.OldCursorPosition.LineIndex, textResult.IsSelectionExist, textResult.SelectionStart.LineIndex, textResult.SelectionEnd.LineIndex);
    }

    public LinesChangeResult MakeForDeleteSelectedLines(DeleteSelectedLinesResult textResult)
    {
        if (!textResult.HasDeleted) return new();
        return _logic.LeftDelete(textResult.OldCursorPosition.LineIndex, true, textResult.SelectionStart.LineIndex, textResult.SelectionEnd.LineIndex + 1);
    }
}
