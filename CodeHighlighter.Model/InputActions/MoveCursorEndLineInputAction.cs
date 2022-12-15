namespace CodeHighlighter.InputActions;

internal interface IMoveCursorEndLineInputAction
{
    void Do(IInputActionContext context);
}

[InputAction]
internal class MoveCursorEndLineInputAction : InputAction, IMoveCursorEndLineInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveEndLine();
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
