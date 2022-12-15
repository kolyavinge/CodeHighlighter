namespace CodeHighlighter.InputActions;

internal interface IMoveCursorPageUpInputAction
{
    void Do(IInputActionContext context);
}

[InputAction]
internal class MoveCursorPageUpInputAction : InputAction, IMoveCursorPageUpInputAction
{
    public void Do(IInputActionContext context)
    {
        var pageSize = context.Viewport.GetLinesCountInViewport();
        context.TextCursor.MovePageUp(pageSize);
        SetSelection(context);
        context.Viewport.CorrectByCursorPosition();
    }
}
