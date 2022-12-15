namespace CodeHighlighter.InputActions;

internal interface IMoveCursorDownInputAction
{
    void Do(IInputActionContext context);
}

[InputAction]
internal class MoveCursorDownInputAction : InputAction, IMoveCursorDownInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveDown();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
