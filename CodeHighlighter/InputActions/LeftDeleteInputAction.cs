namespace CodeHighlighter.InputActions;

internal class LeftDeleteInputAction
{
    public static readonly LeftDeleteInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.LeftDelete();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
