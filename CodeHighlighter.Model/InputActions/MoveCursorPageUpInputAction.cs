namespace CodeHighlighter.InputActions;

internal interface IMoveCursorPageUpInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveCursorPageUpInputAction : InputAction, IMoveCursorPageUpInputAction
{
    public void Do(IInputActionContext context)
    {
        var pageSize = context.Viewport.GetLinesCountInViewport();
        context.TextCursor.MovePageUp(pageSize);
        context.TextSelector.SetSelection();
        context.Viewport.CorrectByCursorPosition();
    }
}
