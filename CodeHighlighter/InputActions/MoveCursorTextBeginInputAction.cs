namespace CodeHighlighter.InputActions;

internal class MoveCursorTextBeginInputAction
{
    public static readonly MoveCursorTextBeginInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveCursorTextBegin();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
