using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

internal class TextRenderLogic
{
    public void DrawText(
        DrawingContext context, Brush defaultForeground, InputModel model, FontSettings fontSettings, TextMeasures textMeasures, Viewport viewport, IViewportContext viewportContext)
    {
        var typeface = new Typeface(fontSettings.FontFamily, fontSettings.FontStyle, fontSettings.FontWeight, fontSettings.FontStretch);
        var startLine = (int)(viewportContext.VerticalScrollBarValue / textMeasures.LineHeight);
        var linesCount = viewport.GetLinesCountInViewport();
        var endLine = Math.Min(startLine + linesCount, model.Text.VisibleLinesCount);
        var offsetY = -(viewportContext.VerticalScrollBarValue % textMeasures.LineHeight);
        for (var lineIndex = startLine; lineIndex < endLine; lineIndex++)
        {
            var lineTokens = model.Tokens.GetTokens(lineIndex);
            foreach (var token in lineTokens)
            {
                var brush = model.TokenColors.GetColorBrushOrNull(token.Kind) ?? defaultForeground;
                var formattedText = new FormattedText(token.Name, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSettings.FontSize, brush, 1.0);
                var offsetX = -viewportContext.HorizontalScrollBarValue + textMeasures.LetterWidth * token.StartColumnIndex;
                context.DrawText(formattedText, new Point(offsetX, offsetY));
            }
            offsetY += textMeasures.LineHeight;
        }
    }
}
