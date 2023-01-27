using CodeHighlighter.Model;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

public static class RenderingModelFactory
{
    public static IRenderingModel MakeModel(ICodeTextBoxModel model, IRenderingContext renderingContext)
    {
        return new RenderingModel(
            new TextRendering(model, renderingContext),
            new TextSelectionRendering(model, renderingContext, new TextSelectionRect()),
            new LinesDecorationRendering(model, renderingContext),
            new HighlightBracketsRendering(model, renderingContext));
    }
}
