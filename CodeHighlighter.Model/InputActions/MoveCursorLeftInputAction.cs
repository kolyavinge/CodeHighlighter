namespace CodeHighlighter.InputActions;

internal interface IMoveCursorLeftInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveCursorLeftInputAction : InputAction, IMoveCursorLeftInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveLeft();
        context.TextSelector.SetSelection();
        context.CursorPositionCorrector.CorrectPosition();
    }
}
