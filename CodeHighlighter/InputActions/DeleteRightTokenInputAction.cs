using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class DeleteRightTokenInputAction
{
    public static readonly DeleteRightTokenInputAction Instance = new();

    public DeleteTokenResult Do(InputActionContext context)
    {
        var result = context.InputModel.DeleteRightToken();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();

        return result;
    }
}
