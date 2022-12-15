namespace CodeHighlighter.InputActions;

internal interface IMoveCursorStartLineInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class MoveCursorStartLineInputAction : InputAction, IMoveCursorStartLineInputAction
{
    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveStartLine();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
