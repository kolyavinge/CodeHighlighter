namespace CodeHighlighter.InputActions;

internal interface IMoveCursorTextEndInputAction
{
    void Do(IInputActionContext context);
}

[InputAction]
internal class MoveCursorTextEndInputAction : InputAction, IMoveCursorTextEndInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveTextEnd();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
