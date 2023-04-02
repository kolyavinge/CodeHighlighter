namespace CodeHighlighter.Core;

public class TextChangedEventArgs : EventArgs
{
    public static readonly TextChangedEventArgs Default = new(default, default);

    public readonly LinesChange AddedLines;
    public readonly LinesChange DeletedLines;

    public TextChangedEventArgs(LinesChange addedLines, LinesChange deletedLines)
    {
        AddedLines = addedLines;
        DeletedLines = deletedLines;
    }
}

public class LinesCountChangedEventArgs : EventArgs
{
    public readonly int LinesCount;

    public LinesCountChangedEventArgs(int linesCount)
    {
        LinesCount = linesCount;
    }
}

public interface ITextEvents
{
    event EventHandler? TextSet;
    event EventHandler<TextChangedEventArgs>? TextChanged;
    event EventHandler<LinesCountChangedEventArgs>? LinesCountChanged;
}

internal interface ITextEventsInternal : ITextEvents
{
    void RaiseTextSet();
    void RaiseTextChanged();
    void RaiseTextChangedAfterAppendNewLine(AppendNewLineResult textResult);
    void RaiseTextChangedAfterInsertText(InsertTextResult textResult);
    void RaiseTextChangedAfterLeftDelete(DeleteResult textResult);
    void RaiseTextChangedAfterRightDelete(DeleteResult textResult);
    void RaiseTextChangedAfterDeleteToken(DeleteTokenResult textResult);
    void RaiseTextChangedAfterDeleteSelectedLines(DeleteSelectedLinesResult textResult);
}

internal class TextEvents : ITextEventsInternal
{
    private readonly IText _text;
    private readonly ITextChangedEventArgsFactory _factory;
    private int _linesCount;

    public event EventHandler? TextSet;
    public event EventHandler<TextChangedEventArgs>? TextChanged;
    public event EventHandler<LinesCountChangedEventArgs>? LinesCountChanged;

    public TextEvents(IText text, ITextChangedEventArgsFactory factory)
    {
        _text = text;
        _factory = factory;
    }

    public void RaiseTextSet()
    {
        TextSet?.Invoke(this, EventArgs.Empty);
        RaiseLinesCountChangedIfNeeded();
    }

    public void RaiseTextChanged()
    {
        TextChanged?.Invoke(this, TextChangedEventArgs.Default);
        RaiseLinesCountChangedIfNeeded();
    }

    public void RaiseTextChangedAfterAppendNewLine(AppendNewLineResult textResult)
    {
        TextChanged?.Invoke(this, _factory.MakeForAppendNewLine(textResult));
        RaiseLinesCountChangedIfNeeded();
    }

    public void RaiseTextChangedAfterInsertText(InsertTextResult textResult)
    {
        TextChanged?.Invoke(this, _factory.MakeForInsertText(textResult));
        RaiseLinesCountChangedIfNeeded();
    }

    public void RaiseTextChangedAfterLeftDelete(DeleteResult textResult)
    {
        TextChanged?.Invoke(this, _factory.MakeForLeftDelete(textResult));
        RaiseLinesCountChangedIfNeeded();
    }

    public void RaiseTextChangedAfterRightDelete(DeleteResult textResult)
    {
        TextChanged?.Invoke(this, _factory.MakeForRightDelete(textResult));
        RaiseLinesCountChangedIfNeeded();
    }

    public void RaiseTextChangedAfterDeleteToken(DeleteTokenResult textResult)
    {
        TextChanged?.Invoke(this, _factory.MakeForDeleteToken(textResult));
        RaiseLinesCountChangedIfNeeded();
    }

    public void RaiseTextChangedAfterDeleteSelectedLines(DeleteSelectedLinesResult textResult)
    {
        TextChanged?.Invoke(this, _factory.MakeForDeleteSelectedLines(textResult));
        RaiseLinesCountChangedIfNeeded();
    }

    private void RaiseLinesCountChangedIfNeeded()
    {
        if (_text.LinesCount != _linesCount)
        {
            _linesCount = _text.LinesCount;
            LinesCountChanged?.Invoke(this, new LinesCountChangedEventArgs(_linesCount));
        }
    }
}
