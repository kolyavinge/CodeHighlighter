using System;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class InsertTextInputAction
{
    public static readonly InsertTextInputAction Instance = new();

    public void Do(string insertedText, InputModel inputModel, Text text, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox, Action raiseTextChanged)
    {
        inputModel.InsertText(insertedText);
        viewport.CorrectByCursorPosition(textCursor);
        viewport.UpdateScrollbarsMaximumValues(text);
        raiseTextChanged();
        codeTextBox?.InvalidateVisual();
    }
}
