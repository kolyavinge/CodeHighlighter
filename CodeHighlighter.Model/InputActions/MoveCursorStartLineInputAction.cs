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
        context.TextSelector.SetSelection();
        context.Viewport.CorrectByCursorPosition();
    }
}
