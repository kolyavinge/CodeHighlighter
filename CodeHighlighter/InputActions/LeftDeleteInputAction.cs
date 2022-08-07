using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class LeftDeleteInputAction
{
    public static readonly LeftDeleteInputAction Instance = new();

    public DeleteResult Do(InputActionContext context)
    {
        var result = context.InputModel.LeftDelete();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();

        return result;
    }
}
