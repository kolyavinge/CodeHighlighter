namespace CodeHighlighter.InputActions;

internal class GotoLineInputAction
{
    public static readonly GotoLineInputAction Instance = new();

    public void Do(InputActionContext context, int lineIndex)
    {
        if (lineIndex < 0) throw new ArgumentException(nameof(lineIndex));
        lineIndex = CalculateLineIndex(lineIndex, context.Text.LinesCount);
        var offsetLine = CalculateOffsetLine(lineIndex, context.Viewport.GetLinesCountInViewport());
        MoveCursorToInputAction.Instance.Do(context, new(lineIndex, 0));
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
