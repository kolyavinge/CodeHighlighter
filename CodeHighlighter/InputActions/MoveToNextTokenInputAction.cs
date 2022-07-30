namespace CodeHighlighter.InputActions;

internal class MoveToNextTokenInputAction
{
    public static readonly MoveToNextTokenInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveToNextToken();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.CodeTextBox?.InvalidateVisual();
    }
}
