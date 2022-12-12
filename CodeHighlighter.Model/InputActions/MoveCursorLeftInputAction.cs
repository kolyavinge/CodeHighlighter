namespace CodeHighlighter.InputActions;

internal class MoveCursorLeftInputAction : InputAction
{
    public static readonly MoveCursorLeftInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveLeft();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
