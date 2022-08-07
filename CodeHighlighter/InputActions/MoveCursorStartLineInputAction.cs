namespace CodeHighlighter.InputActions;

internal class MoveCursorStartLineInputAction
{
    public static readonly MoveCursorStartLineInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveCursorStartLine();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
