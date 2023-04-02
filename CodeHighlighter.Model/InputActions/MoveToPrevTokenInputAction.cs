using CodeHighlighter.Core;

namespace CodeHighlighter.InputActions;

internal interface IMoveToPrevTokenInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveToPrevTokenInputAction : InputAction, IMoveToPrevTokenInputAction
{
    public void Do(IInputActionContext context)
    {
        var navigator = new TokenNavigator();
        var pos = navigator.MoveLeft(context.Text, context.Tokens, context.TextCursor.LineIndex, context.TextCursor.ColumnIndex);
        context.TextCursor.MoveTo(pos);
        context.TextSelector.SetSelection();
        context.CursorPositionCorrector.CorrectPosition();
    }
}
