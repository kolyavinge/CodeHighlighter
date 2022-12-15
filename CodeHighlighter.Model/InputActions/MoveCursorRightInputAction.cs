namespace CodeHighlighter.InputActions;

internal interface IMoveCursorRightInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class MoveCursorRightInputAction : InputAction, IMoveCursorRightInputAction
{
    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveRight();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
