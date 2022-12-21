namespace CodeHighlighter.InputActions;

internal interface IMoveCursorPageDownInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveCursorPageDownInputAction : InputAction, IMoveCursorPageDownInputAction
{
    public void Do(IInputActionContext context)
    {
        var pageSize = context.Viewport.GetLinesCountInViewport();
        context.TextCursor.MovePageDown(pageSize);
        context.TextSelector.SetSelection();
        context.Viewport.CorrectByCursorPosition();
    }
}
