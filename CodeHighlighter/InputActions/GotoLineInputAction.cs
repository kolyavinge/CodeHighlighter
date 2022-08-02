namespace CodeHighlighter.InputActions;

internal class GotoLineInputAction
{
    public static readonly GotoLineInputAction Instance = new();

    public void Do(InputActionContext context, int lineIndex)
    {
        lineIndex = lineIndex < context.Text.LinesCount ? lineIndex : context.Text.LinesCount;
        context.InputModel.MoveCursorTo(new(lineIndex, 0));
        var offsetLine = lineIndex - context.Viewport.GetLinesCountInViewport() / 2;
        if (offsetLine < 0) offsetLine = 0;
        context.ViewportContext.VerticalScrollBarValue = offsetLine * context.TextMeasures.LineHeight;
        context.CodeTextBox?.InvalidateVisual();
        context.CodeTextBox?.Focus();
    }
}
