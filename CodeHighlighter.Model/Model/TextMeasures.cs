namespace CodeHighlighter.Model;

public interface ITextMeasures
{
    event EventHandler? MeasuresUpdated;
    double LetterWidth { get; }
    double LineHeight { get; }
    void UpdateMeasures(double lineHeight, double letterWidth);
}

public class TextMeasures : ITextMeasures
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
