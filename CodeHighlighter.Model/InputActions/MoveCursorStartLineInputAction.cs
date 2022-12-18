namespace CodeHighlighter.InputActions;

internal interface IMoveCursorStartLineInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveCursorStartLineInputAction : InputAction, IMoveCursorStartLineInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveStartLine();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
