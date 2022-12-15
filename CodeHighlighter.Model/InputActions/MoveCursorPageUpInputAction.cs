namespace CodeHighlighter.InputActions;

internal interface IMoveCursorPageUpInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class MoveCursorPageUpInputAction : InputAction, IMoveCursorPageUpInputAction
{
    public static readonly MoveCursorPageUpInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        var pageSize = context.Viewport.GetLinesCountInViewport();
        context.TextCursor.MovePageUp(pageSize);
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
