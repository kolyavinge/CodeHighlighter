namespace CodeHighlighter.InputActions;

internal class MoveCursorRightInputAction
{
    public static readonly MoveCursorRightInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.MoveCursorRight();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
    }
}
