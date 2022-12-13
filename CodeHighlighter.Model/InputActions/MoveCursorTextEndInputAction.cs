namespace CodeHighlighter.InputActions;

internal class MoveCursorTextEndInputAction : InputAction
{
    public static readonly MoveCursorTextEndInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveTextEnd();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
