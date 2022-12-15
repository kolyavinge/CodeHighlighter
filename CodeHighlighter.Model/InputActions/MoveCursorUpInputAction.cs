namespace CodeHighlighter.InputActions;

internal interface IMoveCursorUpInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class MoveCursorUpInputAction : InputAction, IMoveCursorUpInputAction
{
    public static readonly MoveCursorUpInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveUp();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
