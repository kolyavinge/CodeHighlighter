using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class ReplaceTextInputAction
{
    public static readonly ReplaceTextInputAction Instance = new();

    public void Do(InputActionContext context, CursorPosition start, CursorPosition end, string insertedText)
    {
        context.InputModel.MoveCursorTo(start);
        context.InputModel.ActivateSelection();
        context.InputModel.MoveCursorTo(end);
        context.InputModel.CompleteSelection();
        context.InputModel.InsertText(insertedText);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
