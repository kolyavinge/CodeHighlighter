namespace CodeHighlighter.InputActions;

internal interface IMoveCursorEndLineInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveCursorEndLineInputAction : InputAction, IMoveCursorEndLineInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveEndLine();
        context.TextSelector.SetSelection();
        context.Viewport.CorrectByCursorPosition();
    }
}
