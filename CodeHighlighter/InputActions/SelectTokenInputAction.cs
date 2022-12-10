using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class SelectTokenInputAction : InputAction
{
    public static readonly SelectTokenInputAction Instance = new();

    public void Do(InputActionContext context, CursorPosition position)
    {
        var selector = new TokenSelector();
        var range = selector.GetSelection(context.Tokens, position);
        context.TextCursor.MoveTo(new(position.LineIndex, range.EndCursorColumnIndex));
        context.TextSelection.Reset();
        context.TextSelection.StartPosition = new(position.LineIndex, range.StartCursorColumnIndex);
        context.TextSelection.EndPosition = new(position.LineIndex, range.EndCursorColumnIndex);
    }
}
