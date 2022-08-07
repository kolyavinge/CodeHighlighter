using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class AppendCharInputAction
{
    public static readonly AppendCharInputAction Instance = new();

    public AppendCharResult Do(InputActionContext context, char ch)
    {
        var result = context.InputModel.AppendChar(ch);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();

        return result;
    }
}
