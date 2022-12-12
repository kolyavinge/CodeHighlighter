namespace CodeHighlighter.InputActions;

internal class MoveCursorDownInputAction : InputAction
{
    public static readonly MoveCursorDownInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveDown();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
