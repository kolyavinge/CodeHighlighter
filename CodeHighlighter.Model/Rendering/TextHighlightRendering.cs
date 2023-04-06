using CodeHighlighter.Common;
using CodeHighlighter.Core;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

public interface ITextHighlightRendering
{
    void Render();
}

internal class TextHighlightRendering : ITextHighlightRendering
{
    private readonly ICodeTextBoxModel _model;
    private readonly ICodeTextBoxRenderingContext _renderingContext;
    private readonly ITextSelectionRect _textSelectionRect;

    public TextHighlightRendering(
        ICodeTextBoxModel model,
        ICodeTextBoxRenderingContext renderingContext,
        ITextSelectionRect textSelectionRect)
    {
        _model = model;
        _renderingContext = renderingContext;
        _textSelectionRect = textSelectionRect;
    }

    public void Render()
    {
        foreach (var highlight in _model.TextHighlighter.Highlights)
        {
            var selectedLines = _model.TextHighlighter.GetSelectedLines(highlight);
            _textSelectionRect
                .GetCalculatedRects(selectedLines, _model.TextMeasures, _model.Viewport.ActualHeight, _model.Viewport.HorizontalScrollBarValue, _model.Viewport.VerticalScrollBarValue)
                .Each(rect => _renderingContext.DrawRectangle(highlight.Color, rect));
        }
    }
}
