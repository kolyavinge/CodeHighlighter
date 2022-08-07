using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class RightDeleteInputAction
{
    public static readonly RightDeleteInputAction Instance = new();

    public DeleteResult Do(InputActionContext context)
    {
        var result = context.InputModel.RightDelete();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();

        return result;
    }
}
