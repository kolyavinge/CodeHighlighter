namespace CodeHighlighter.InputActions;

internal class MoveToPrevTokenInputAction
{
    public static readonly MoveToPrevTokenInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveToPrevToken();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
