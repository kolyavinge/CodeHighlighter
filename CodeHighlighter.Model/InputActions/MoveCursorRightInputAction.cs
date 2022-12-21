namespace CodeHighlighter.InputActions;

internal interface IMoveCursorRightInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveCursorRightInputAction : InputAction, IMoveCursorRightInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveRight();
        context.TextSelector.SetSelection();
        context.Viewport.CorrectByCursorPosition();
    }
}
