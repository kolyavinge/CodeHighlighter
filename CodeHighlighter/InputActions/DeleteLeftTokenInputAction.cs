using System;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class DeleteLeftTokenInputAction
{
    public static readonly DeleteLeftTokenInputAction Instance = new();

    public void Do(InputModel inputModel, Text text, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox, Action raiseTextChanged)
    {
        inputModel.DeleteLeftToken();
        viewport.CorrectByCursorPosition(textCursor);
        viewport.UpdateScrollbarsMaximumValues(text);
        raiseTextChanged();
        codeTextBox?.InvalidateVisual();
    }
}
