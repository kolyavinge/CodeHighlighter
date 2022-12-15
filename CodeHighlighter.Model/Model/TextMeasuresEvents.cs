namespace CodeHighlighter.Model;

public class TextMeasuresEvents
{
    private readonly ITextMeasures _textMeasures;
    private double _lineHeight;
    private double _letterWidth;

    public event EventHandler<LineHeightChangedEventArgs>? LineHeightChanged;

    public event EventHandler<LetterWidthChangedEventArgs>? LetterWidthChanged;

    public TextMeasuresEvents(ITextMeasures textMeasures)
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

public class LineHeightChangedEventArgs : EventArgs
{
    public double LineHeight { get; }
    public LineHeightChangedEventArgs(double lineHeight)
    {
        LineHeight = lineHeight;
    }
}

public class LetterWidthChangedEventArgs : EventArgs
{
    public double LetterWidth { get; }
    public LetterWidthChangedEventArgs(double letterWidth)
    {
        LetterWidth = letterWidth;
    }
}
