using System;

namespace CodeHighlighter.Model;

internal class TextMeasuresEvents
{
    private readonly TextMeasures _textMeasures;
    private double _lineHeight;

    public event EventHandler<LineHeightChangedEventArgs>? LineHeightChanged;

    public TextMeasuresEvents(TextMeasures textMeasures)
    {
        _textMeasures = textMeasures;
        _textMeasures.MeasuresUpdated += OnMeasuresUpdated;
        _lineHeight = _textMeasures.LineHeight;
    }

    private void OnMeasuresUpdated(object? sender, EventArgs e)
    {
        if (_textMeasures.LineHeight != _lineHeight)
        {
            _lineHeight = _textMeasures.LineHeight;
            LineHeightChanged?.Invoke(this, new LineHeightChangedEventArgs(_lineHeight));
        }
    }
}

internal class LineHeightChangedEventArgs : EventArgs
{
    public double LineHeight { get; }
    public LineHeightChangedEventArgs(double lineHeight)
    {
        LineHeight = lineHeight;
    }
}
