namespace CodeHighlighter.Model;

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
    event EventHandler? TextChanged;
    event EventHandler<LinesCountChangedEventArgs>? LinesCountChanged;
    void RaiseTextSet();
    void RaiseTextChanged();
}

internal class TextEvents : ITextEvents
{
    private readonly IText _text;
    private int _linesCount;

    public event EventHandler? TextSet;
    public event EventHandler? TextChanged;
    public event EventHandler<LinesCountChangedEventArgs>? LinesCountChanged;

    public TextEvents(IText text)
    {
        _text = text;
    }

    public void RaiseTextSet()
    {
        TextSet?.Invoke(this, EventArgs.Empty);
        RaiseLinesCountChangedIfNeeded();
    }

    public void RaiseTextChanged()
    {
        TextChanged?.Invoke(this, EventArgs.Empty);
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

public class LinesCountChangedEventArgs : EventArgs
{
    public int LinesCount { get; }
    public LinesCountChangedEventArgs(int linesCount)
    {
        LinesCount = linesCount;
    }
}
