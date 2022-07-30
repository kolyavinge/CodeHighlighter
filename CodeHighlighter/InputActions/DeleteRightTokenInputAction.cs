namespace CodeHighlighter.InputActions;

internal class DeleteRightTokenInputAction
{
    public static readonly DeleteRightTokenInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.DeleteRightToken();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
