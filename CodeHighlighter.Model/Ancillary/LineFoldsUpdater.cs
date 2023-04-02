using CodeHighlighter.Core;

namespace CodeHighlighter.Ancillary;

internal interface ILineFoldsUpdater
{
    void UpdateAppendNewLine(AppendNewLineResult textResult);
    void UpdateInsertText(InsertTextResult textResult);
    void UpdateLeftDelete(DeleteResult textResult);
    void UpdateRightDelete(DeleteResult textResult);
    void UpdateDeleteToken(DeleteTokenResult textResult);
    void UpdateDeleteSelectedLines(DeleteSelectedLinesResult textResult);
}

internal class LineFoldsUpdater : ILineFoldsUpdater
{
    private readonly ILineFolds _folds;
    private readonly IEditTextResultToLinesChangeConverter _converter;

    public LineFoldsUpdater(
        ILineFolds folds,
        IEditTextResultToLinesChangeConverter converter)
    {
        _folds = folds;
        _converter = converter;
    }

    public void UpdateAppendNewLine(AppendNewLineResult textResult)
    {
        var result = _converter.MakeForAppendNewLine(textResult);
        Update(result);
    }

    public void UpdateInsertText(InsertTextResult textResult)
    {
        var result = _converter.MakeForInsertText(textResult);
        Update(result);
    }

    public void UpdateLeftDelete(DeleteResult textResult)
    {
        var result = _converter.MakeForLeftDelete(textResult);
        Update(result);
    }

    public void UpdateRightDelete(DeleteResult textResult)
    {
        var result = _converter.MakeForRightDelete(textResult);
        Update(result);
    }

    public void UpdateDeleteToken(DeleteTokenResult textResult)
    {
        var result = _converter.MakeForDeleteToken(textResult);
        Update(result);
    }

    public void UpdateDeleteSelectedLines(DeleteSelectedLinesResult textResult)
    {
        var result = _converter.MakeForDeleteSelectedLines(textResult);
        Update(result);
    }

    private void Update(LinesChangeResult result)
    {
        _folds.UpdateAfterLineAdd(result.AddedLines.StartLineIndex, result.AddedLines.LinesCount);
        _folds.UpdateAfterLineDelete(result.DeletedLines.StartLineIndex, result.DeletedLines.LinesCount);
    }
}
