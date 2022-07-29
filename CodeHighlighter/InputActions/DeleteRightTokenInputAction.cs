using System;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class DeleteRightTokenInputAction
{
    public static readonly DeleteRightTokenInputAction Instance = new();

    public void Do(InputModel inputModel, Text text, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox, Action raiseTextChanged)
    {
        inputModel.DeleteRightToken();
        viewport.CorrectByCursorPosition(textCursor);
        viewport.UpdateScrollbarsMaximumValues(text);
        raiseTextChanged();
        codeTextBox?.InvalidateVisual();
    }
}
