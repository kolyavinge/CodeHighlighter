namespace CodeHighlighter.InputActions;

internal interface IMoveCursorTextBeginInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveCursorTextBeginInputAction : InputAction, IMoveCursorTextBeginInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveTextBegin();
        context.TextSelector.SetSelection();
        context.Viewport.CorrectByCursorPosition();
    }
}
