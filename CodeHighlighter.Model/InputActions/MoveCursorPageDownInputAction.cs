namespace CodeHighlighter.InputActions;

internal interface IMoveCursorPageDownInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveCursorPageDownInputAction : InputAction, IMoveCursorPageDownInputAction
{
    public void Do(IInputActionContext context)
    {
        var newLineIndex = context.Viewport.GetCursorLineIndexAfterScrollPageDown(context.TextCursor.LineIndex);
        context.TextCursor.MoveTo(new(newLineIndex, context.TextCursor.ColumnIndex));
        context.TextSelector.SetSelection();
        context.CursorPositionCorrector.CorrectPosition();
    }
}
