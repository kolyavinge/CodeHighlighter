using CodeHighlighter.Core;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

public interface ILinesDecorationRendering
{
    void Render();
}

internal class LinesDecorationRendering : ILinesDecorationRendering
{
    private readonly ICodeTextBox _model;
    private readonly ICodeTextBoxRenderingContext _renderingContext;
    private readonly ILineNumberGenerator _lineNumberGenerator;

    public LinesDecorationRendering(
        ICodeTextBox model,
        ICodeTextBoxRenderingContext renderingContext,
        ILineNumberGenerator lineNumberGenerator)
    {
        _model = model;
        _renderingContext = renderingContext;
        _lineNumberGenerator = lineNumberGenerator;
    }

    public void Render()
    {
        var linesDecorationCollection = _model.LinesDecoration;
        if (!linesDecorationCollection.AnyItems) return;

        var textMeasures = _model.TextMeasures;
        var viewport = _model.Viewport;

        foreach (var line in _lineNumberGenerator.GetLineNumbers(viewport.ActualHeight, viewport.VerticalScrollBarValue, textMeasures.LineHeight, _model.TextLines.Count))
        {
            var lineDecoration = linesDecorationCollection[line.LineIndex];
            if (lineDecoration != null)
            {
                var x = 0.0;
                var y = line.LineIndex * textMeasures.LineHeight - viewport.VerticalScrollBarValue;
                var height = textMeasures.LineHeight;
                var background = lineDecoration.Background;
                _renderingContext.DrawRectangle(background, new(x, y, _model.Viewport.ActualWidth, height));
            }
        }
    }
}
