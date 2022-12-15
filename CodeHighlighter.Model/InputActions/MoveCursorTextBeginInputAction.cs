namespace CodeHighlighter.InputActions;

internal interface IMoveCursorTextBeginInputAction
{
    void Do(IInputActionContext context);
}

[InputAction]
internal class MoveCursorTextBeginInputAction : InputAction, IMoveCursorTextBeginInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveTextBegin();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
