using System.Globalization;
using System.Windows;
using System.Windows.Media;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

internal class TextRenderLogic
{
    public void DrawText(CodeTextBoxModel model, FontSettings fontSettings, DrawingContext context, Brush defaultForeground)
    {
        var textMeasures = model.TextMeasures;
        var viewport = model.Viewport;
        var typeface = new Typeface(fontSettings.FontFamily, fontSettings.FontStyle, fontSettings.FontWeight, fontSettings.FontStretch);
        foreach (var line in LineNumber.GetLineNumbers(viewport.ActualHeight, viewport.VerticalScrollBarValue, textMeasures.LineHeight, model.Text.LinesCount))
        {
            var lineTokens = model.Tokens.GetTokens(line.Index);
            foreach (var token in lineTokens)
            {
                var tokenColor = model.TokensColors.GetColor(token.Kind);
                var brush = tokenColor != null
                    ? new SolidColorBrush(new() { R = tokenColor.Value.R, G = tokenColor.Value.G, B = tokenColor.Value.B, A = tokenColor.Value.A })
                    : defaultForeground;
                var formattedText = new FormattedText(token.Name, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSettings.FontSize, brush, 1.0);
                var offsetX = -viewport.HorizontalScrollBarValue + textMeasures.LetterWidth * token.StartColumnIndex;
                context.DrawText(formattedText, new Point(offsetX, line.OffsetY));
            }
        }
    }
}
