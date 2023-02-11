namespace CodeHighlighter.InputActions;

internal interface IMoveCursorPageUpInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveCursorPageUpInputAction : InputAction, IMoveCursorPageUpInputAction
{
    public void Do(IInputActionContext context)
    {
        var newLineIndex = context.PageScroller.GetCursorLineIndexAfterScrollPageUp(context.TextCursor.LineIndex);
        context.TextCursor.MoveTo(new(newLineIndex, context.TextCursor.ColumnIndex));
        context.TextSelector.SetSelection();
        context.CursorPositionCorrector.CorrectPosition();
    }
}
