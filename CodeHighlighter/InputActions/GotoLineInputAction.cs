using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class GotoLineInputAction
{
    public static readonly GotoLineInputAction Instance = new();

    public void Do(int lineIndex, InputModel inputModel, Text text, TextMeasures textMeasures, Viewport viewport, IViewportContext viewportContext, ICodeTextBox? codeTextBox)
    {
        lineIndex = lineIndex < text.LinesCount ? lineIndex : text.LinesCount;
        inputModel.MoveCursorTo(lineIndex, 0);
        var offsetLine = lineIndex - viewport.GetLinesCountInViewport() / 2;
        if (offsetLine < 0) offsetLine = 0;
        viewportContext.VerticalScrollBarValue = offsetLine * textMeasures.LineHeight;
        codeTextBox?.InvalidateVisual();
        codeTextBox?.Focus();
    }
}
