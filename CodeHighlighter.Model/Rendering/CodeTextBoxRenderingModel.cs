﻿namespace CodeHighlighter.Rendering;

public interface ICodeTextBoxRenderingModel
{
    ITextRendering Text { get; }
    ITextSelectionRendering TextSelection { get; }
    ILinesDecorationRendering LinesDecoration { get; }
    IHighlightBracketsRendering HighlightBrackets { get; }
    ILineGapRendering LineGap { get; }
    IActivatedLineFoldsRendering ActivatedLineFolds { get; }
}

internal class CodeTextBoxRenderingModel : ICodeTextBoxRenderingModel
{
    public ITextRendering Text { get; }
    public ITextSelectionRendering TextSelection { get; }
    public ILinesDecorationRendering LinesDecoration { get; }
    public IHighlightBracketsRendering HighlightBrackets { get; }
    public ILineGapRendering LineGap { get; }
    public IActivatedLineFoldsRendering ActivatedLineFolds { get; }

    public CodeTextBoxRenderingModel(
        ITextRendering textRendering,
        ITextSelectionRendering textSelectionRendering,
        ILinesDecorationRendering linesDecorationRendering,
        IHighlightBracketsRendering highlightBracketsRendering,
        ILineGapRendering lineGap,
        IActivatedLineFoldsRendering activatedLineFolds)
    {
        Text = textRendering;
        TextSelection = textSelectionRendering;
        LinesDecoration = linesDecorationRendering;
        HighlightBrackets = highlightBracketsRendering;
        LineGap = lineGap;
        ActivatedLineFolds = activatedLineFolds;
    }
}
