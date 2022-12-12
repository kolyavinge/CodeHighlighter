namespace CodeHighlighter.InputActions;

internal class MoveCursorPageDownInputAction : InputAction
{
    public static readonly MoveCursorPageDownInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        var pageSize = context.Viewport.GetLinesCountInViewport();
        context.TextCursor.MovePageDown(pageSize);
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
