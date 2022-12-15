namespace CodeHighlighter.InputActions;

internal interface IMoveCursorPageDownInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class MoveCursorPageDownInputAction : InputAction, IMoveCursorPageDownInputAction
{
    public static readonly MoveCursorPageDownInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        var pageSize = context.Viewport.GetLinesCountInViewport();
        context.TextCursor.MovePageDown(pageSize);
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
