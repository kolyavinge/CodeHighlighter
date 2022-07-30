namespace CodeHighlighter.InputActions;

internal class DeleteLeftTokenInputAction
{
    public static readonly DeleteLeftTokenInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.DeleteLeftToken();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
