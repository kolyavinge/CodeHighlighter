namespace CodeHighlighter.InputActions;

internal class MoveCursorPageDownInputAction
{
    public static readonly MoveCursorPageDownInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveCursorPageDown(context.Viewport.GetLinesCountInViewport());
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
