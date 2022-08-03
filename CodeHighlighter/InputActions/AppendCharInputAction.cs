namespace CodeHighlighter.InputActions;

internal class AppendCharInputAction
{
    public static readonly AppendCharInputAction Instance = new();

    public void Do(InputActionContext context, char ch)
    {
        context.InputModel.AppendChar(ch);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
