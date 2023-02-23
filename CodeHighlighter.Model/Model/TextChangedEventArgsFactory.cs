namespace CodeHighlighter.Model;

internal interface ITextChangedEventArgsFactory
{
    TextChangedEventArgs MakeForAppendNewLine(AppendNewLineResult textResult);
    TextChangedEventArgs MakeForInsertText(InsertTextResult textResult);
    TextChangedEventArgs MakeForLeftDelete(DeleteResult textResult);
    TextChangedEventArgs MakeForRightDelete(DeleteResult textResult);
    TextChangedEventArgs MakeForDeleteToken(DeleteTokenResult textResult);
    TextChangedEventArgs MakeForDeleteSelectedLines(DeleteSelectedLinesResult textResult);
}

internal class TextChangedEventArgsFactory : ITextChangedEventArgsFactory
{
    private readonly ITextLinesChangingLogic _logic;

    public TextChangedEventArgsFactory(ITextLinesChangingLogic textLinesChangingLogic)
    {
        _logic = textLinesChangingLogic;
    }

    public TextChangedEventArgs MakeForAppendNewLine(AppendNewLineResult textResult)
    {
        var result = _logic.AppendNewLine(textResult.OldCursorPosition.LineIndex, textResult.SelectionStart.LineIndex, textResult.SelectionEnd.LineIndex);

        return new(result.AddedLines, result.DeletedLines);
    }

    public TextChangedEventArgs MakeForInsertText(InsertTextResult textResult)
    {
        if (!textResult.HasInserted) return TextChangedEventArgs.Default;
        var result = _logic.InsertText(
            textResult.InsertStartPosition.LineIndex, textResult.InsertEndPosition.LineIndex, textResult.SelectionStart.LineIndex, textResult.SelectionEnd.LineIndex);

        return new(result.AddedLines, result.DeletedLines);
    }

    public TextChangedEventArgs MakeForLeftDelete(DeleteResult textResult)
    {
        if (!(textResult.IsSelectionExist && textResult.HasDeleted) && !textResult.CharDeleteResult.IsLineDeleted) return TextChangedEventArgs.Default;
        var result = _logic.LeftDelete(textResult.OldCursorPosition.LineIndex, textResult.IsSelectionExist, textResult.SelectionStart.LineIndex, textResult.SelectionEnd.LineIndex);

        return new(result.AddedLines, result.DeletedLines);
    }

    public TextChangedEventArgs MakeForRightDelete(DeleteResult textResult)
    {
        if (!(textResult.IsSelectionExist && textResult.HasDeleted) && !textResult.CharDeleteResult.IsLineDeleted) return TextChangedEventArgs.Default;
        var result = _logic.RightDelete(textResult.OldCursorPosition.LineIndex, textResult.IsSelectionExist, textResult.SelectionStart.LineIndex, textResult.SelectionEnd.LineIndex);

        return new(result.AddedLines, result.DeletedLines);
    }

    public TextChangedEventArgs MakeForDeleteToken(DeleteTokenResult textResult)
    {
        if (!textResult.HasDeleted) return TextChangedEventArgs.Default;
        var result = _logic.LeftDelete(textResult.OldCursorPosition.LineIndex, textResult.IsSelectionExist, textResult.SelectionStart.LineIndex, textResult.SelectionEnd.LineIndex);

        return new(result.AddedLines, result.DeletedLines);
    }

    public TextChangedEventArgs MakeForDeleteSelectedLines(DeleteSelectedLinesResult textResult)
    {
        if (!textResult.HasDeleted) return TextChangedEventArgs.Default;
        var result = _logic.LeftDelete(textResult.OldCursorPosition.LineIndex, true, textResult.SelectionStart.LineIndex, textResult.SelectionEnd.LineIndex + 1);

        return new(result.AddedLines, result.DeletedLines);
    }
}
