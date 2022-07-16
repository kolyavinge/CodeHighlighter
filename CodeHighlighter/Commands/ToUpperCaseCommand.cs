using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class ToUpperCaseCommand : InputCommand
{
    public ToUpperCaseCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.SetSelectedTextCase(TextCase.Upper);
        _context.TextBox.InvalidateVisual();
    }
}
