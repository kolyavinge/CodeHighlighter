namespace CodeHighlighter.InputActions;

internal class InsertTextInputAction
{
    public static readonly InsertTextInputAction Instance = new();

    public void Do(InputActionContext context, string insertedText)
    {
        context.InputModel.InsertText(insertedText);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
