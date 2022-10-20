using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class SetTextInputAction
{
    public static readonly SetTextInputAction Instance = new();

    public SetTextResult Do(InputActionContext context, string text)
    {
        var result = context.InputModel.SetText(text);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextSet();

        return result;
    }
}
