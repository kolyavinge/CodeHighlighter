using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

public class TextInputCommandParameter
{
    public string InputText { get; }

    public TextInputCommandParameter(string inputText)
    {
        InputText = inputText;
    }
}

internal class TextInputCommand : InputCommand
{
    private static readonly HashSet<char> _notAllowedSymbols = new(new[] { '\n', '\r', '\b', '\u001B' });

    public TextInputCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        var p = (TextInputCommandParameter)parameter;
        var text = p.InputText.Where(ch => !_notAllowedSymbols.Contains(ch)).ToList();
        if (!text.Any()) return;
        foreach (var ch in text)
        {
            _context.Model.AppendChar(ch);
        }
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
