using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace CodeHighlighter.Model;

public class TextMeasures
{
    private readonly FontSettings _fontSettings;

    internal event EventHandler? MeasuresUpdated;

    internal TextMeasures(FontSettings fontSettings)
    {
        _fontSettings = fontSettings;
        UpdateMeasures();
    }

    // for unit tests only
    internal TextMeasures(double lineHeight, double letterWidth)
    {
        LineHeight = lineHeight;
        LetterWidth = letterWidth;
        _fontSettings = new();
    }

    public double LineHeight { get; private set; }

    public double LetterWidth { get; private set; }

    internal void UpdateMeasures()
    {
        var formattedText = new FormattedText("A", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, MakeTypeface(), _fontSettings.FontSize, Brushes.Black, 1.0);
        LineHeight = Math.Round(formattedText.Height);
        LetterWidth = formattedText.WidthIncludingTrailingWhitespace;
        MeasuresUpdated?.Invoke(this, EventArgs.Empty);
    }

    private Typeface MakeTypeface() => new(_fontSettings.FontFamily, _fontSettings.FontStyle, _fontSettings.FontWeight, _fontSettings.FontStretch);
}
