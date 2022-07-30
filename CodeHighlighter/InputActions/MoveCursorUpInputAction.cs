namespace CodeHighlighter.InputActions;

internal class MoveCursorUpInputAction
{
    public static readonly MoveCursorUpInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveCursorUp();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.CodeTextBox?.InvalidateVisual();
    }
}
