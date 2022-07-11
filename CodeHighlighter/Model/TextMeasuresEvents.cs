using System;

namespace CodeHighlighter.Model;

internal class TextMeasuresEvents
{
    private readonly TextMeasures _textMeasures;
    private double _lineHeight;
    private double _letterWidth;

    public event EventHandler<LineHeightChangedEventArgs>? LineHeightChanged;

    public event EventHandler<LetterWidthChangedEventArgs>? LetterWidthChanged;

    public TextMeasuresEvents(TextMeasures textMeasures)
    {
        _textMeasures = textMeasures;
        _textMeasures.MeasuresUpdated += OnMeasuresUpdated;
        _lineHeight = _textMeasures.LineHeight;
        _letterWidth = _textMeasures.LetterWidth;
    }

    private void OnMeasuresUpdated(object? sender, EventArgs e)
    {
        if (_textMeasures.LineHeight != _lineHeight)
        {
            _lineHeight = _textMeasures.LineHeight;
            LineHeightChanged?.Invoke(this, new(_lineHeight));
        }

        if (_textMeasures.LetterWidth != _letterWidth)
        {
            _letterWidth = _textMeasures.LetterWidth;
            LetterWidthChanged?.Invoke(this, new(_letterWidth));
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

internal class LetterWidthChangedEventArgs : EventArgs
{
    public double LetterWidth { get; }
    public LetterWidthChangedEventArgs(double letterWidth)
    {
        LetterWidth = letterWidth;
    }
}
