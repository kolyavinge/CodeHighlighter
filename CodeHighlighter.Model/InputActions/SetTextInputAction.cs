﻿using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal interface ISetTextInputAction
{
    SetTextResult Do(InputActionContext context, string text);
}

[InputAction]
internal class SetTextInputAction : InputAction, ISetTextInputAction
{
    public SetTextResult Do(InputActionContext context, string text)
    {
        var result = SetText(context, text);
        context.Viewport.CorrectByCursorPosition();
        context.Viewport.UpdateScrollbarsMaximumValues();
        context.TextEvents.RaiseTextSet();

        return result;
    }

    private SetTextResult SetText(InputActionContext context, string text)
    {
        var oldCursorPosition = context.TextCursor.Position;
        var oldText = context.Text.TextContent;
        context.TextCursor.MoveTextBegin();
        context.Text.TextContent = text;
        SetTokens(context);

        return new(oldCursorPosition, oldText, text);
    }
}
