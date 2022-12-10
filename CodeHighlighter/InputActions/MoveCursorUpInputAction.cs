namespace CodeHighlighter.InputActions;

internal class MoveCursorUpInputAction : InputAction
{
    public static readonly MoveCursorUpInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveUp();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
