namespace CodeHighlighter.Model;

public interface ITextMeasures
{
    double LineHeight { get; }
    double LetterWidth { get; }
}

internal interface ITextMeasuresInternal : ITextMeasures
{
    event EventHandler? MeasuresUpdated;
    void UpdateMeasures(double lineHeight, double letterWidth);
}

internal class TextMeasures : ITextMeasuresInternal
{
    public event EventHandler? MeasuresUpdated;

    public double LineHeight { get; private set; }

    public double LetterWidth { get; private set; }

    public void UpdateMeasures(double lineHeight, double letterWidth)
    {
        LineHeight = Math.Round(lineHeight);
        LetterWidth = letterWidth;
        MeasuresUpdated?.Invoke(this, EventArgs.Empty);
    }
}
