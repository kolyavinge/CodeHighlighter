namespace CodeHighlighter.InputActions;

internal interface IMoveCursorRightInputAction
{
    void Do(IInputActionContext context);
}

[InputAction]
internal class MoveCursorRightInputAction : InputAction, IMoveCursorRightInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveRight();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
