using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class InsertTextInputAction
{
    public static readonly InsertTextInputAction Instance = new();

    public InsertTextResult Do(InputActionContext context, string insertedText)
    {
        var insertResult = context.InputModel.InsertText(insertedText);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();

        return insertResult;
    }
}
