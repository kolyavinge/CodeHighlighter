namespace CodeHighlighter.InputActions;

internal class MoveCursorRightInputAction : InputAction
{
    public static readonly MoveCursorRightInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveRight();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
