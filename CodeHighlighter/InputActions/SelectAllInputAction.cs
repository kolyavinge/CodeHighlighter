namespace CodeHighlighter.InputActions;

internal class SelectAllInputAction
{
    public static readonly SelectAllInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.SelectAll();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
