using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

internal class TextRenderLogic
{
    private readonly CodeTextBoxModel _model;
    private readonly FontSettings _fontSettings;
    private readonly TextMeasures _textMeasures;
    private readonly Viewport _viewport;
    private readonly IViewportContext _viewportContext;

    public TextRenderLogic(CodeTextBoxModel model, FontSettings fontSettings, TextMeasures textMeasures, Viewport viewport, IViewportContext viewportContext)
    {
        _model = model;
        _fontSettings = fontSettings;
        _textMeasures = textMeasures;
        _viewport = viewport;
        _viewportContext = viewportContext;
    }

    public void DrawText(DrawingContext context, Brush defaultForeground)
    {
        var typeface = new Typeface(_fontSettings.FontFamily, _fontSettings.FontStyle, _fontSettings.FontWeight, _fontSettings.FontStretch);
        var startLine = (int)(_viewportContext.VerticalScrollBarValue / _textMeasures.LineHeight);
        var linesCount = _viewport.GetLinesCountInViewport();
        var endLine = Math.Min(startLine + linesCount, _model.Text.VisibleLinesCount);
        var offsetY = -(_viewportContext.VerticalScrollBarValue % _textMeasures.LineHeight);
        for (var lineIndex = startLine; lineIndex < endLine; lineIndex++)
        {
            var lineTokens = _model.Tokens.GetTokens(lineIndex);
            foreach (var token in lineTokens)
            {
                var brush = _model.TokenColors.GetColorBrushOrNull(token.Kind) ?? defaultForeground;
                var formattedText = new FormattedText(token.Name, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, _fontSettings.FontSize, brush, 1.0);
                var offsetX = -_viewportContext.HorizontalScrollBarValue + _textMeasures.LetterWidth * token.StartColumnIndex;
                context.DrawText(formattedText, new Point(offsetX, offsetY));
            }
            offsetY += _textMeasures.LineHeight;
        }
    }
}
