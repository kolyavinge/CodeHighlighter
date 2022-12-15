namespace CodeHighlighter.InputActions;

internal interface IMoveCursorTextBeginInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class MoveCursorTextBeginInputAction : InputAction, IMoveCursorTextBeginInputAction
{
    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveTextBegin();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
