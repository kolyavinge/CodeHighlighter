namespace CodeHighlighter.InputActions;

internal class MoveSelectedLinesUpInputAction
{
    public static readonly MoveSelectedLinesUpInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveSelectedLinesUp();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.CodeTextBox?.InvalidateVisual();
    }
}
