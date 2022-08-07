namespace CodeHighlighter.InputActions;

internal class MoveCursorPageUpInputAction
{
    public static readonly MoveCursorPageUpInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveCursorPageUp(context.Viewport.GetLinesCountInViewport());
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
