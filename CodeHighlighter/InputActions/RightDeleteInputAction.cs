using System;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class RightDeleteInputAction
{
    public static readonly RightDeleteInputAction Instance = new();

    public void Do(InputModel inputModel, Text text, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox, Action raiseTextChanged)
    {
        inputModel.RightDelete();
        viewport.CorrectByCursorPosition(textCursor);
        viewport.UpdateScrollbarsMaximumValues(text);
        raiseTextChanged();
        codeTextBox?.InvalidateVisual();
    }
}
