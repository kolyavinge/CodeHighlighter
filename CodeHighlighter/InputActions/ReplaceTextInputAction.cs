namespace CodeHighlighter.InputActions;

internal class ReplaceTextInputAction
{
    public static readonly ReplaceTextInputAction Instance = new();

    public void Do(
        InputActionContext context,
        int cursorStartLineIndex,
        int cursorStartColumnIndex,
        int cursorEndLineIndex,
        int cursorEndColumnIndex,
        string insertedText)
    {
        context.InputModel.MoveCursorTo(cursorStartLineIndex, cursorStartColumnIndex);
        context.InputModel.ActivateSelection();
        context.InputModel.MoveCursorTo(cursorEndLineIndex, cursorEndColumnIndex);
        context.InputModel.CompleteSelection();
        context.InputModel.InsertText(insertedText);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
