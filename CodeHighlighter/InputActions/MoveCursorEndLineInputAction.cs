namespace CodeHighlighter.InputActions;

internal class MoveCursorEndLineInputAction : InputAction
{
    public static readonly MoveCursorEndLineInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveEndLine();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
