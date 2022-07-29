using System;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class LeftDeleteInputAction
{
    public static readonly LeftDeleteInputAction Instance = new();

    public void Do(InputModel inputModel, Text text, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox, Action raiseTextChanged)
    {
        inputModel.LeftDelete();
        viewport.CorrectByCursorPosition(textCursor);
        viewport.UpdateScrollbarsMaximumValues(text);
        raiseTextChanged();
        codeTextBox?.InvalidateVisual();
    }
}
