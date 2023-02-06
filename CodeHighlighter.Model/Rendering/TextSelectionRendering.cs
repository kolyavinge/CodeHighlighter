using CodeHighlighter.Model;
using CodeHighlighter.Utils;

namespace CodeHighlighter.Rendering;

public interface ITextSelectionRendering
{
    void Render(object platformColor);
}

internal class TextSelectionRendering : ITextSelectionRendering
{
    private readonly ICodeTextBoxModel _model;
    private readonly IRenderingContext _renderingContext;
    private readonly ITextSelectionRect _textSelectionRect;

    public TextSelectionRendering(
        ICodeTextBoxModel model, IRenderingContext renderingContext, ITextSelectionRect textSelectionRect)
    {
        _model = model;
        _renderingContext = renderingContext;
        _textSelectionRect = textSelectionRect;
    }

    public void Render(object platformColor)
    {
        var selectedLines = _model.TextSelection.GetSelectedLines();
        _textSelectionRect
            .GetCalculatedRects(selectedLines, _model.TextMeasures, _model.Viewport.ActualHeight, _model.Viewport.HorizontalScrollBarValue, _model.Viewport.VerticalScrollBarValue)
            .Each(rect => _renderingContext.DrawRectangle(platformColor, rect));
    }
}
