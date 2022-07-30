namespace CodeHighlighter.InputActions;

internal class MoveCursorEndLineInputAction
{
    public static readonly MoveCursorEndLineInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveCursorEndLine();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.CodeTextBox?.InvalidateVisual();
    }
}
