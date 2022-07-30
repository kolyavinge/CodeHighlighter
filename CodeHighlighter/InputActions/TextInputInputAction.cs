using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.InputActions;

internal class TextInputInputAction
{
    public static readonly TextInputInputAction Instance = new();

    private static readonly HashSet<char> _notAllowedSymbols = new(new[] { '\n', '\r', '\b', '\u001B' });

    public void Do(InputActionContext context, string inputText)
    {
        var inputTextList = inputText.Where(ch => !_notAllowedSymbols.Contains(ch)).ToList();
        if (!inputTextList.Any()) return;
        foreach (var ch in inputTextList) context.InputModel.AppendChar(ch);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
