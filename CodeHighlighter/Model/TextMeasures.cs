using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace CodeHighlighter.Model;

internal class TextMeasures
{
    private readonly IFontSettings _fontSettings;

    public TextMeasures(IFontSettings fontSettings)
    {
        _fontSettings = fontSettings;
        UpdateMeasures();
    }

    // for unit tests only
    internal TextMeasures(double lineHeight, double letterWidth)
    {
        LineHeight = lineHeight;
        LetterWidth = letterWidth;
    }

    public double LineHeight { get; private set; }

    public double LetterWidth { get; private set; }

    public double HalfLetterWidth => LetterWidth / 2.0;

    public void UpdateMeasures()
    {
        var formattedText = new FormattedText("A", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, MakeTypeface(), _fontSettings.FontSize, Brushes.Black, 1.0);
        LineHeight = Math.Round(formattedText.Height);
        LetterWidth = formattedText.WidthIncludingTrailingWhitespace;
    }

    private Typeface MakeTypeface() => new(_fontSettings.FontFamily, _fontSettings.FontStyle, _fontSettings.FontWeight, _fontSettings.FontStretch);
}
