namespace CodeHighlighter.InputActions;

internal interface IMoveCursorStartLineInputAction
{
    void Do(IInputActionContext context);
}

[InputAction]
internal class MoveCursorStartLineInputAction : InputAction, IMoveCursorStartLineInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveStartLine();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
