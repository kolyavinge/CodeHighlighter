using System;
using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class TextInputInputAction
{
    public static readonly TextInputInputAction Instance = new();

    private static readonly HashSet<char> _notAllowedSymbols = new(new[] { '\n', '\r', '\b', '\u001B' });

    public void Do(string inputText, InputModel inputModel, Text text, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox, Action raiseTextChanged)
    {
        var inputTextList = inputText.Where(ch => !_notAllowedSymbols.Contains(ch)).ToList();
        if (!inputTextList.Any()) return;
        foreach (var ch in inputTextList) inputModel.AppendChar(ch);
        viewport.CorrectByCursorPosition(textCursor);
        viewport.UpdateScrollbarsMaximumValues(text);
        raiseTextChanged();
        codeTextBox?.InvalidateVisual();
    }
}
