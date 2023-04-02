using CodeHighlighter.Core;

namespace CodeHighlighter.InputActions;

internal interface ISelectTokenInputAction
{
    void Do(IInputActionContext context, CursorPosition position);
}

internal class SelectTokenInputAction : InputAction, ISelectTokenInputAction
{
    public void Do(IInputActionContext context, CursorPosition position)
    {
        var selector = new TokenSelector();
        var range = selector.GetSelection(context.Tokens, position);
        context.TextCursor.MoveTo(new(position.LineIndex, range.EndCursorColumnIndex));
        context.TextSelection.Reset();
        context.TextSelection.StartPosition = new(position.LineIndex, range.StartCursorColumnIndex);
        context.TextSelection.EndPosition = new(position.LineIndex, range.EndCursorColumnIndex);
    }
}
