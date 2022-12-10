namespace CodeHighlighter.InputActions;

internal class MoveCursorStartLineInputAction : InputAction
{
    public static readonly MoveCursorStartLineInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveStartLine();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
