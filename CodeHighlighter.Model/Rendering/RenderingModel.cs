namespace CodeHighlighter.Rendering;

public interface IRenderingModel
{
    ITextRendering Text { get; }
    ITextSelectionRendering TextSelection { get; }
    ILinesDecorationRendering LinesDecoration { get; }
    IHighlightBracketsRendering HighlightBrackets { get; }
}

internal class RenderingModel : IRenderingModel
{
    public ITextRendering Text { get; }
    public ITextSelectionRendering TextSelection { get; }
    public ILinesDecorationRendering LinesDecoration { get; }
    public IHighlightBracketsRendering HighlightBrackets { get; }

    public RenderingModel(
        ITextRendering textRendering,
        ITextSelectionRendering textSelectionRendering,
        ILinesDecorationRendering linesDecorationRendering,
        IHighlightBracketsRendering highlightBracketsRendering)
    {
        Text = textRendering;
        TextSelection = textSelectionRendering;
        LinesDecoration = linesDecorationRendering;
        HighlightBrackets = highlightBracketsRendering;
    }
}
