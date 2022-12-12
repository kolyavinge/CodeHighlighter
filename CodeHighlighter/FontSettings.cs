using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace CodeHighlighter;

internal class FontSettings
{
    public double FontSize { get; set; }
    public FontFamily FontFamily { get; set; }
    public FontStyle FontStyle { get; set; }
    public FontWeight FontWeight { get; set; }
    public FontStretch FontStretch { get; set; }

    public double LineHeight
    {
        get
        {
            var formattedText = MakeText();
            return formattedText.Height;
        }
    }

    public double LetterWidth
    {
        get
        {
            var formattedText = MakeText();
            return formattedText.Width;
        }
    }

    public FontSettings()
    {
        FontFamily = new FontFamily("Consolas");
        FontSize = 12;
    }

    private FormattedText MakeText() => new("A", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, MakeTypeface(), FontSize, Brushes.Black, 1.0);

    private Typeface MakeTypeface() => new(FontFamily, FontStyle, FontWeight, FontStretch);
}
