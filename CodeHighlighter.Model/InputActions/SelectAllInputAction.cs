namespace CodeHighlighter.InputActions;

internal class SelectAllInputAction : InputAction
{
    public static readonly SelectAllInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.TextSelection.InProgress = false;
        context.TextSelection.StartPosition = new(0, 0);
        context.TextSelection.EndPosition = new(context.Text.LinesCount - 1, context.Text.GetLine(context.Text.LinesCount - 1).Length);
        context.TextCursor.MoveTextEnd();
        context.Viewport.CorrectByCursorPosition();
    }
}
