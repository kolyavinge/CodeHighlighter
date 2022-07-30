namespace CodeHighlighter.InputActions;

internal class RightDeleteInputAction
{
    public static readonly RightDeleteInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.RightDelete();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
