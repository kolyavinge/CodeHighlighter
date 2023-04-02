namespace CodeHighlighter.Core;

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
    private readonly IEditTextResultToLinesChangeConverter _converter;

    public TextChangedEventArgsFactory(IEditTextResultToLinesChangeConverter converter)
    {
        _converter = converter;
    }

    public TextChangedEventArgs MakeForAppendNewLine(AppendNewLineResult textResult)
    {
        var result = _converter.MakeForAppendNewLine(textResult);
        return new(result.AddedLines, result.DeletedLines);
    }

    public TextChangedEventArgs MakeForInsertText(InsertTextResult textResult)
    {
        var result = _converter.MakeForInsertText(textResult);
        return new(result.AddedLines, result.DeletedLines);
    }

    public TextChangedEventArgs MakeForLeftDelete(DeleteResult textResult)
    {
        var result = _converter.MakeForLeftDelete(textResult);
        return new(result.AddedLines, result.DeletedLines);
    }

    public TextChangedEventArgs MakeForRightDelete(DeleteResult textResult)
    {
        var result = _converter.MakeForRightDelete(textResult);
        return new(result.AddedLines, result.DeletedLines);
    }

    public TextChangedEventArgs MakeForDeleteToken(DeleteTokenResult textResult)
    {
        var result = _converter.MakeForDeleteToken(textResult);
        return new(result.AddedLines, result.DeletedLines);
    }

    public TextChangedEventArgs MakeForDeleteSelectedLines(DeleteSelectedLinesResult textResult)
    {
        var result = _converter.MakeForDeleteSelectedLines(textResult);
        return new(result.AddedLines, result.DeletedLines);
    }
}
