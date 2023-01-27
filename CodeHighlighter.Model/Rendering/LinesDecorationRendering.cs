using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

public interface ILinesDecorationRendering
{
    void Render(double lineWidth);
}

internal class LinesDecorationRendering : ILinesDecorationRendering
{
    private readonly ICodeTextBoxModel _model;
    private readonly IRenderingContext _renderingContext;

    public LinesDecorationRendering(ICodeTextBoxModel model, IRenderingContext renderingContext)
    {
        _model = model;
        _renderingContext = renderingContext;
    }

    public void Render(double lineWidth)
    {
        var linesDecorationCollection = _model.LinesDecoration;
        if (!linesDecorationCollection.AnyItems) return;

        var textMeasures = _model.TextMeasures;
        var viewport = _model.Viewport;

        foreach (var line in LineNumber.GetLineNumbers(viewport.ActualHeight, viewport.VerticalScrollBarValue, textMeasures.LineHeight, _model.TextLinesCount))
        {
            var lineDecoration = linesDecorationCollection[line.Index];
            if (lineDecoration != null)
            {
                var x = 0.0;
                var y = line.Index * textMeasures.LineHeight - viewport.VerticalScrollBarValue;
                var width = lineWidth;
                var height = textMeasures.LineHeight;
                var background = lineDecoration.Background;
                _renderingContext.DrawRectangle(background, new(x, y, width, height));
            }
        }
    }
}
