using CodeHighlighter.Infrastructure;

namespace CodeHighlighter.InputActions;

internal interface IGotoLineInputAction
{
    void Do(IInputActionContext context, int lineIndex);
}

[InputAction]
internal class GotoLineInputAction : InputAction, IGotoLineInputAction
{
    private readonly IInputActionsFactory _inputActionsFactory;

    public GotoLineInputAction(IInputActionsFactory inputActionsFactory)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public void Do(IInputActionContext context, int lineIndex)
    {
        if (lineIndex < 0) throw new ArgumentException(nameof(lineIndex));
        lineIndex = CalculateLineIndex(lineIndex, context.Text.LinesCount);
        var offsetLine = CalculateOffsetLine(lineIndex, context.Viewport.GetLinesCountInViewport());
        _inputActionsFactory.Get<IMoveCursorToInputAction>().Do(context, new(lineIndex, 0));
        context.Viewport.VerticalScrollBarValue = CalculateVerticalScrollBarValue(offsetLine, context.TextMeasures.LineHeight);
    }

    public int CalculateLineIndex(int lineIndex, int textLinesCount)
    {
        return lineIndex < textLinesCount ? lineIndex : textLinesCount;
    }

    public int CalculateOffsetLine(int lineIndex, int linesCountInViewport)
    {
        var offsetLine = lineIndex - linesCountInViewport / 2;
        if (offsetLine < 0) offsetLine = 0;

        return offsetLine;
    }

    public double CalculateVerticalScrollBarValue(int offsetLine, double lineHeight)
    {
        return offsetLine * lineHeight;
    }
}
