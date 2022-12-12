namespace CodeHighlighter.InputActions;

internal class MoveCursorPageUpInputAction : InputAction
{
    public static readonly MoveCursorPageUpInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        var pageSize = context.Viewport.GetLinesCountInViewport();
        context.TextCursor.MovePageUp(pageSize);
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
