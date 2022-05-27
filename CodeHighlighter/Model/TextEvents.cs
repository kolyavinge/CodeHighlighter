using System;

namespace CodeHighlighter.Model;

internal class TextEvents
{
    private readonly IText _text;
    private int _linesCount;

    public event EventHandler<LinesCountChangedEventArgs>? LinesCountChanged;

    public TextEvents(IText text)
    {
        _text = text;
        _linesCount = _text.LinesCount;
        _text.TextChanged += OnTextChanged;
    }

    private void OnTextChanged(object? sender, EventArgs e)
    {
        if (_text.LinesCount != _linesCount)
        {
            _linesCount = _text.LinesCount;
            LinesCountChanged?.Invoke(this, new LinesCountChangedEventArgs(_linesCount));
        }
    }
}

internal class LinesCountChangedEventArgs : EventArgs
{
    public int LinesCount { get; }
    public LinesCountChangedEventArgs(int linesCount)
    {
        LinesCount = linesCount;
    }
}
