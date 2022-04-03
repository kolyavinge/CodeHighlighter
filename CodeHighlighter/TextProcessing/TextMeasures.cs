using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace CodeHighlighter.TextProcessing
{
    internal class TextMeasures
    {
        private readonly FontSettings _fontSettings;

        public TextMeasures(FontSettings fontSettings)
        {
            _fontSettings = fontSettings;
            UpdateMeasures();
        }

        public double LineHeight { get; private set; }

        public double LetterWidth { get; private set; }

        public void UpdateMeasures()
        {
            var formattedText = new FormattedText("A", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, MakeTypeface(), _fontSettings.FontSize, Brushes.Black, 1.0);
            LineHeight = Math.Round(formattedText.Height);
            LetterWidth = formattedText.WidthIncludingTrailingWhitespace;
        }

        private Typeface MakeTypeface() => new(_fontSettings.FontFamily, _fontSettings.FontStyle, _fontSettings.FontWeight, _fontSettings.FontStretch);
    }
}
