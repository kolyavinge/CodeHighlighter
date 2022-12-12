namespace CodeHighlighter.Model;

public class TextMeasures
{
    internal event EventHandler? MeasuresUpdated;

    public double LineHeight { get; private set; }

    public double LetterWidth { get; private set; }

    public void UpdateMeasures(double lineHeight, double letterWidth)
    {
        LineHeight = Math.Round(lineHeight);
        LetterWidth = letterWidth;
        MeasuresUpdated?.Invoke(this, EventArgs.Empty);
    }
}
