namespace CodeHighlighter.InputActions;

internal interface IMoveCursorLeftInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class MoveCursorLeftInputAction : InputAction, IMoveCursorLeftInputAction
{
    public static readonly MoveCursorLeftInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveLeft();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
