namespace CodeHighlighter.InputActions;

internal class MoveCursorLeftInputAction
{
    public static readonly MoveCursorLeftInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveCursorLeft();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.CodeTextBox?.InvalidateVisual();
    }
}
