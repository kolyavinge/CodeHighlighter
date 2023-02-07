namespace CodeHighlighter.InputActions;

internal interface IMoveCursorUpInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveCursorUpInputAction : InputAction, IMoveCursorUpInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveUp();
        context.TextSelector.SetSelection();
        context.CursorPositionCorrector.CorrectPosition();
    }
}
