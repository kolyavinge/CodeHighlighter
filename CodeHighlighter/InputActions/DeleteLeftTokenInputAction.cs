using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class DeleteLeftTokenInputAction
{
    public static readonly DeleteLeftTokenInputAction Instance = new();

    public DeleteTokenResult Do(InputActionContext context)
    {
        var result = context.InputModel.DeleteLeftToken();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();

        return result;
    }
}
