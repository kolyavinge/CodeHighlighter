namespace CodeHighlighter.InputActions;

internal interface IMoveCursorUpInputAction
{
    void Do(IInputActionContext context);
}

[InputAction]
internal class MoveCursorUpInputAction : InputAction, IMoveCursorUpInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveUp();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
