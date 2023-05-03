namespace CodeHighlighter.Rendering;

public interface ICodeTextBoxRendering
{
    ITextRendering Text { get; }
    ITextSelectionRendering TextSelection { get; }
    ILinesDecorationRendering LinesDecoration { get; }
    IHighlightBracketsRendering HighlightBrackets { get; }
    ILineGapRendering LineGap { get; }
    IActivatedLineFoldsRendering ActivatedLineFolds { get; }
    ITextHighlightRendering TextHighlight { get; }
}

internal class CodeTextBoxRendering : ICodeTextBoxRendering
{
    public ITextRendering Text { get; }
    public ITextSelectionRendering TextSelection { get; }
    public ILinesDecorationRendering LinesDecoration { get; }
    public IHighlightBracketsRendering HighlightBrackets { get; }
    public ILineGapRendering LineGap { get; }
    public IActivatedLineFoldsRendering ActivatedLineFolds { get; }
    public ITextHighlightRendering TextHighlight { get; }

    public CodeTextBoxRendering(
        ITextRendering textRendering,
        ITextSelectionRendering textSelectionRendering,
        ILinesDecorationRendering linesDecorationRendering,
        IHighlightBracketsRendering highlightBracketsRendering,
        ILineGapRendering lineGap,
        IActivatedLineFoldsRendering activatedLineFolds,
        ITextHighlightRendering textHighlight)
    {
        Text = textRendering;
        TextSelection = textSelectionRendering;
        LinesDecoration = linesDecorationRendering;
        HighlightBrackets = highlightBracketsRendering;
        LineGap = lineGap;
        ActivatedLineFolds = activatedLineFolds;
        TextHighlight = textHighlight;
    }
}
