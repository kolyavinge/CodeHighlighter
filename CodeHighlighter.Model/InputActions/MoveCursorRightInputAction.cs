namespace CodeHighlighter.InputActions;

internal interface IMoveCursorRightInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class MoveCursorRightInputAction : InputAction, IMoveCursorRightInputAction
{
    public static readonly MoveCursorRightInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveRight();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
