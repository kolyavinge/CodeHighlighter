using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class DeleteSelectedLinesInputAction
{
    public static readonly DeleteSelectedLinesInputAction Instance = new();

    public DeleteSelectedLinesResult Do(InputActionContext context)
    {
        var result = context.InputModel.DeleteSelectedLines();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();

        return result;
    }
}
