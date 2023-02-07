﻿using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal interface ISetTextInputAction
{
    SetTextResult Do(IInputActionContext context, string text);
}

internal class SetTextInputAction : InputAction, ISetTextInputAction
{
    public SetTextResult Do(IInputActionContext context, string text)
    {
        var result = SetText(context, text);
        context.CursorPositionCorrector.CorrectPosition();
        context.Viewport.UpdateScrollBarsMaximumValues();
        context.TextEvents.RaiseTextSet();

        return result;
    }

    private SetTextResult SetText(IInputActionContext context, string text)
    {
        var oldCursorPosition = context.TextCursor.Position;
        var oldText = context.Text.TextContent;
        context.TextCursor.MoveTextBegin();
        context.Text.TextContent = text;
        SetTokens(context);

        return new(oldCursorPosition, oldText, text);
    }
}
