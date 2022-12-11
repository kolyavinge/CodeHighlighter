using System.Globalization;
using System.Windows;
using System.Windows.Media;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

internal class TextRenderLogic
{
    public void DrawText(CodeTextBoxModel model, DrawingContext context, Brush defaultForeground)
    {
        var fontSettings = model.FontSettings;
        var textMeasures = model.TextMeasures;
        var viewport = model.Viewport;
        var typeface = new Typeface(fontSettings.FontFamily, fontSettings.FontStyle, fontSettings.FontWeight, fontSettings.FontStretch);
        foreach (var line in LineNumber.GetLineNumbers(viewport.ActualHeight, viewport.VerticalScrollBarValue, textMeasures.LineHeight, model.Text.LinesCount))
        {
            var lineTokens = model.Tokens.GetTokens(line.Index);
            foreach (var token in lineTokens)
            {
                var brush = model.TokensColors.GetColorBrushOrNull(token.Kind) ?? defaultForeground;
                var formattedText = new FormattedText(token.Name, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSettings.FontSize, brush, 1.0);
                var offsetX = -viewport.HorizontalScrollBarValue + textMeasures.LetterWidth * token.StartColumnIndex;
                context.DrawText(formattedText, new Point(offsetX, line.OffsetY));
            }
        }
    }
}
