using CodeHighlighter.Core;

namespace CodeHighlighter.InputActions;

internal interface IMoveCursorToInputAction
{
    void Do(IInputActionContext context, CursorPosition position);
}

internal class MoveCursorToInputAction : InputAction, IMoveCursorToInputAction
{
    public void Do(IInputActionContext context, CursorPosition position)
    {
        context.TextCursor.MoveTo(position);
        context.TextSelector.SetSelection();
        context.CursorPositionCorrector.CorrectPosition();
    }
}
