namespace CodeHighlighter.InputActions;

internal interface IMoveCursorEndLineInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class MoveCursorEndLineInputAction : InputAction, IMoveCursorEndLineInputAction
{
    public static readonly MoveCursorEndLineInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveEndLine();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
