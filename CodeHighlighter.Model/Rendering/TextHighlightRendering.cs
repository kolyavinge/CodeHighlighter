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
    private readonly ICodeTextBox _model;
    private readonly ICodeTextBoxRenderingContext _renderingContext;
    private readonly ITextSelectionRect _textSelectionRect;

    public TextHighlightRendering(
        ICodeTextBox model,
        ICodeTextBoxRenderingContext renderingContext,
        ITextSelectionRect textSelectionRect)
    {
        _model = model;
        _renderingContext = renderingContext;
        _textSelectionRect = textSelectionRect;
    }

    public void Render()
    {
        foreach (var item in _model.TextHighlighter.GetSelectedLines())
        {
            var (highlight, selectedLines) = item;
            _textSelectionRect
                .GetCalculatedRects(selectedLines, _model.TextMeasures, _model.Viewport.ActualHeight, _model.Viewport.HorizontalScrollBarValue, _model.Viewport.VerticalScrollBarValue)
                .Each(rect => _renderingContext.DrawRectangle(highlight.Color, rect));
        }
    }
}
