namespace CodeHighlighter.Core;

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
        return _logic.AppendNewLine(textResult.OldCursorPosition, textResult.SelectionStart, textResult.SelectionEnd);
    }

    public LinesChangeResult MakeForInsertText(InsertTextResult textResult)
    {
        if (!textResult.HasInserted) return new();
        return _logic.InsertText(textResult.InsertStartPosition, textResult.InsertEndPosition, textResult.SelectionStart, textResult.SelectionEnd);
    }

    public LinesChangeResult MakeForLeftDelete(DeleteResult textResult)
    {
        if (!(textResult.IsSelectionExist && textResult.HasDeleted) && !textResult.CharDeleteResult.IsLineDeleted) return new();
        return _logic.LeftDelete(textResult.NewCursorPosition, textResult.SelectionStart, textResult.SelectionEnd);
    }

    public LinesChangeResult MakeForRightDelete(DeleteResult textResult)
    {
        if (!(textResult.IsSelectionExist && textResult.HasDeleted) && !textResult.CharDeleteResult.IsLineDeleted) return new();
        return _logic.RightDelete(textResult.OldCursorPosition, textResult.SelectionStart, textResult.SelectionEnd);
    }

    public LinesChangeResult MakeForDeleteToken(DeleteTokenResult textResult)
    {
        if (!textResult.HasDeleted) return new();
        return _logic.LeftDelete(textResult.OldCursorPosition, textResult.SelectionStart, textResult.SelectionEnd);
    }

    public LinesChangeResult MakeForDeleteSelectedLines(DeleteSelectedLinesResult textResult)
    {
        if (!textResult.HasDeleted) return new();
        return _logic.LeftDelete(textResult.OldCursorPosition, new(textResult.SelectionStart.LineIndex, 0), new(textResult.SelectionEnd.LineIndex + 1, 0));
    }
}
