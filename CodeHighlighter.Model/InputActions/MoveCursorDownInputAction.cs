namespace CodeHighlighter.InputActions;

internal interface IMoveCursorDownInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class MoveCursorDownInputAction : InputAction, IMoveCursorDownInputAction
{
    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveDown();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
