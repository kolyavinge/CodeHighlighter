namespace CodeHighlighter.InputActions;

internal class MoveSelectedLinesDownInputAction
{
    public static readonly MoveSelectedLinesDownInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveSelectedLinesDown();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.CodeTextBox?.InvalidateVisual();
    }
}
