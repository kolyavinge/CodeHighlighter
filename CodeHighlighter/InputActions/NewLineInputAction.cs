using System;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class NewLineInputAction
{
    public static readonly NewLineInputAction Instance = new();

    public void Do(InputModel inputModel, Text text, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox, Action raiseTextChanged)
    {
        inputModel.NewLine();
        viewport.CorrectByCursorPosition(textCursor);
        viewport.UpdateScrollbarsMaximumValues(text);
        raiseTextChanged();
        codeTextBox?.InvalidateVisual();
    }
}
