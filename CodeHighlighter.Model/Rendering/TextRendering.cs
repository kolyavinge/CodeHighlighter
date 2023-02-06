using CodeHighlighter.Common;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

public interface ITextRendering
{
    void Render();
}

internal class TextRendering : ITextRendering
{
    private readonly ICodeTextBoxModel _model;
    private readonly IRenderingContext _renderingContext;
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;

    public TextRendering(
        ICodeTextBoxModel model,
        IRenderingContext renderingContext,
        IExtendedLineNumberGenerator lineNumberGenerator)
    {
        _model = model;
        _renderingContext = renderingContext;
        _lineNumberGenerator = lineNumberGenerator;
    }

    public void Render()
    {
        var textMeasures = _model.TextMeasures;
        var viewport = _model.Viewport;
        foreach (var line in _lineNumberGenerator.GetLineNumbers(viewport.ActualHeight, viewport.VerticalScrollBarValue, textMeasures.LineHeight, _model.TextLinesCount))
        {
            var lineTokens = _model.Tokens.GetTokens(line.LineIndex);
            foreach (var token in lineTokens)
            {
                var tokenColor = _model.TokensColors.GetColor(token.Kind);
                var offsetX = -viewport.HorizontalScrollBarValue + textMeasures.LetterWidth * token.StartColumnIndex;
                _renderingContext.DrawText(token.Name, new Point(offsetX, line.OffsetY), tokenColor);
            }
        }
    }
}
