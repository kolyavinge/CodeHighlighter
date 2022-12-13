using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveToNextTokenInputAction : InputAction
{
    public static readonly MoveToNextTokenInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        var navigator = new TokenNavigator();
        var pos = navigator.MoveRight(context.Text, context.Tokens, context.TextCursor.LineIndex, context.TextCursor.ColumnIndex);
        context.TextCursor.MoveTo(pos);
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
