namespace CodeHighlighter.InputActions;

internal class MoveCursorDownInputAction
{
    public static readonly MoveCursorDownInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveCursorDown();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.CodeTextBox?.InvalidateVisual();
    }
}
