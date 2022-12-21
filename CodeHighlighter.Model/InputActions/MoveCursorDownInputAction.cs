namespace CodeHighlighter.InputActions;

internal interface IMoveCursorDownInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveCursorDownInputAction : InputAction, IMoveCursorDownInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveDown();
        context.TextSelector.SetSelection();
        context.Viewport.CorrectByCursorPosition();
    }
}
