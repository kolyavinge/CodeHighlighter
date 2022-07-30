namespace CodeHighlighter.InputActions;

internal class NewLineInputAction
{
    public static readonly NewLineInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.NewLine();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
