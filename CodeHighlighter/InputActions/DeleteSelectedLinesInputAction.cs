namespace CodeHighlighter.InputActions;

internal class DeleteSelectedLinesInputAction
{
    public static readonly DeleteSelectedLinesInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.DeleteSelectedLines();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
