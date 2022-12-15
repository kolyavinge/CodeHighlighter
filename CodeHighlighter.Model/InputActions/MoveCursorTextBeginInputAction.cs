namespace CodeHighlighter.InputActions;

internal class MoveCursorTextBeginInputAction : InputAction
{
    public static readonly MoveCursorTextBeginInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextCursor.MoveTextBegin();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
