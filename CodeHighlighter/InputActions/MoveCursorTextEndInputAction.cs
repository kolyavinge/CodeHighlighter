namespace CodeHighlighter.InputActions;

internal class MoveCursorTextEndInputAction
{
    public static readonly MoveCursorTextEndInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveCursorTextEnd();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
