using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class AppendNewLineInputAction
{
    public static readonly AppendNewLineInputAction Instance = new();

    public AppendNewLineResult Do(InputActionContext context)
    {
        var result = context.InputModel.AppendNewLine();
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();

        return result;
    }
}
