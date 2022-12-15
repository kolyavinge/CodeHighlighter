using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal interface IMoveToPrevTokenInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class MoveToPrevTokenInputAction : InputAction, IMoveToPrevTokenInputAction
{
    public void Do(InputActionContext context)
    {
        var navigator = new TokenNavigator();
        var pos = navigator.MoveLeft(context.Text, context.Tokens, context.TextCursor.LineIndex, context.TextCursor.ColumnIndex);
        context.TextCursor.MoveTo(pos);
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
