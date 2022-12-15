namespace CodeHighlighter.InputActions;

internal interface IMoveCursorTextEndInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class MoveCursorTextEndInputAction : InputAction, IMoveCursorTextEndInputAction
{
    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveTextEnd();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
