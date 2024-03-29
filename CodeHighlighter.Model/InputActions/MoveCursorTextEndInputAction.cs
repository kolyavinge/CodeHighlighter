﻿namespace CodeHighlighter.InputActions;

internal interface IMoveCursorTextEndInputAction
{
    void Do(IInputActionContext context);
}

internal class MoveCursorTextEndInputAction : InputAction, IMoveCursorTextEndInputAction
{
    public void Do(IInputActionContext context)
    {
        context.TextCursor.MoveTextEnd();
        context.TextSelector.SetSelection();
        context.CursorPositionCorrector.CorrectPosition();
    }
}
